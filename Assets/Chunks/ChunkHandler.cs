using Assets.Chunks.Threading;
using Assets.Collections;
using Assets.Shapes;
using Assets.Threading;
using Assets.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Chunks
{
    public static class ChunkHandler
    {

        static ConcurrentDictionary<Vector3, Chunk> ChunkCollection = new ConcurrentDictionary<Vector3, Chunk>();
        static Dictionary<Vector3, AssignedWorkContainer> AssignedWork = new Dictionary<Vector3, AssignedWorkContainer>();

        static Dictionary<JobType, ThreadedChunkWork> NonChunkSpecificWork = new Dictionary<JobType, ThreadedChunkWork>();

        static bool PlayerPositionHasChanged = false;
        static Vector3 PlayerChunkPos = new Vector3(0, 0, 0);
        public static int ViewDistance = 15;

        public static Vector3[] GetLoadedChunkPositions
        {
            get
            {
                return ChunkCollection.Keys.ToArray();
            }
        }

        public static Vector3 PlayerPosition
        {
            get
            {
                return PlayerChunkPos;
            }
        }

        public static bool PlayerHasMoved
        {
            get
            {
                return PlayerPositionHasChanged;
            }
        }

        public static void SetBlockWithoutDeterminingShape(Vector3 ChunkPos, int x, int y, int z, int blockID)
        {

        }

        public static void SetBlock(Vector3 ChunkPos, int x, int y, int z, int blockID)
        {

        }

        public static void SetShape(Vector3 ChunkPos, int x, int y, int z, int shapeID)
        {

        }

        static void RemoveChunk(Vector3 chunkPos)
        {
            Chunk c;

            if (ChunkCollection.TryGetValue(chunkPos,out c))
            {
                ChunkCollection.TryRemove(chunkPos, out c);
                c.Dispose();
            }

            c = null;
        }

        public static Block GetBlock(Vector3 ChunkPos, int x, int y, int z)
        {
            Chunk c;
            Block b;

            if (TryGetChunk(ChunkPos + Chunk.GetChunkPos(x, y, z), out c))
            {
                return c.GetBlock(Chunk.GetBlockPos(new Vector3(x,y,z)));
            }

            BlockCollection.TryGetBlock(0, out b);
            return b;
            
        }

        public static Shape GetShape(Vector3 ChunkPos, int x, int y, int z)
        {
            Chunk c;

            if (TryGetChunk(ChunkPos + Chunk.GetChunkPos(x, y, z), out c))
            {
                return c.GetShape(Chunk.GetBlockPos(new Vector3(x, y, z)));
            }

            return null;
        }

        public static bool TryGetChunk(int x, int y, int z, out Chunk c)
        {
            c = null;
            return ChunkCollection.TryGetValue(new Vector3(x,y,z),out c);
        }

        public static bool TryGetChunk(Vector3 chunkPos, out Chunk c)
        {
            c = null;
            return ChunkCollection.TryGetValue(chunkPos, out c);
        }

        public static bool TryAddChunk(Chunk c)
        {
            try
            {
                return ChunkCollection.TryAdd(c.ChunkPos, c);
                
            } catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                return false;
            }
        }

        public static void TryDrawingChunk(Vector3 chunkPos)
        {
            AssignedWorkContainer awc = GetWorkContainer(chunkPos);
            //Check if the chunk is already drawn or if it requires redrawing.
            if (!awc.ContainsWork(JobType.Draw))
            {
                ThreadedPendingDrawingWork tpdw = new ThreadedPendingDrawingWork(chunkPos);
                awc.AddAssignedJob(JobType.Draw, new AssignedJob(chunkPos, JobType.Draw, tpdw));
                ThreadHandler.EnqueuWork(tpdw);
            }
        }

        public static void DrawChunk(Vector3 chunkPos)
        {
            AssignedWorkContainer awc = GetWorkContainer(chunkPos);
            
            if (!awc.ContainsWork(JobType.Draw))
            {
                Chunk c;
                TryGetChunk(chunkPos, out c);
                ThreadedSubChunkDrawingWork[] workArr = c.GetSubChunksToDraw();
                if (workArr.Length > 0)
                {
                    awc.AddAssignedJob(JobType.Draw, new AssignedJob(chunkPos, JobType.Draw, workArr));
                    ThreadHandler.EnqueuWork(workArr);
                } else
                {
                    ChunkGameObjectHandler.UpdateMesh(c.ChunkPos, c.GetMesh()).transform.position = c.ChunkPos * Chunk.CHUNKSIZE;
                }
            }
        }

        static void CheckCompletedDrawingWork(CompletedSubChunkDrawingWork cscdw)
        {
            AssignedWorkContainer awc;
            AssignedWork.TryGetValue(cscdw.ChunkPos, out awc);
            AssignedJob aj = awc.TryGetAssignedJob(JobType.Draw);

            aj.CompletedCount += 1;
            Chunk c;
            ChunkHandler.TryGetChunk(aj.OwnerChunkPos, out c);

            if (aj.IsCompleted)
            {
                awc.RemoveAssignedJob(JobType.Draw);
                ChunkGameObjectHandler.UpdateMesh(c.ChunkPos, c.GetMesh()).transform.position = c.ChunkPos * Chunk.CHUNKSIZE;
            }
        }

        static void CheckCompletedViewDistanceLoaderWork(CompletedViewDistanceLoaderWork cvdlw)
        {
            foreach (Vector3 v in cvdlw.ChunksToGenerate)
            {
                GenerateChunk(v);
            }

            foreach (Vector3 v in cvdlw.ChunksToDraw)
            {
                TryDrawingChunk(v);
            }

            NonChunkSpecificWork.Remove(JobType.LoadViewDistance);

            if (cvdlw.ChunksToGenerate.Length > 0 || PlayerPositionHasChanged)
            {
                DrawAllChunksWithinViewDistance();
                PlayerPositionHasChanged = false;
            }

            UnloadChunksOutsideViewDistance();

        }

        static void UnloadChunksOutsideViewDistance()
        {
            
            ThreadedChunkWork tcw;

            if (!NonChunkSpecificWork.TryGetValue(JobType.UnloadOutsideViewDistance, out tcw))
            {
                ThreadedViewDistanceUnloaderWork tvduw = new ThreadedViewDistanceUnloaderWork(PlayerChunkPos, ViewDistance, GetLoadedChunkPositions,ChunkGameObjectHandler.GetDrawnChunkPositions);
                NonChunkSpecificWork.Add(JobType.UnloadOutsideViewDistance, tvduw);
                ThreadHandler.EnqueuWork(tvduw);
            }
            
        }

        static void CheckCompletedViewDistanceUnloaderWork(CompletedThreadedViewDistanceUnloader ctcduw)
        {
            foreach (Vector3 v in ctcduw.DrawnChunksToUnload)
            {
                ChunkGameObjectHandler.PoolChunk(v);
            }
            int prevCount = ChunkCollection.Count;

            foreach (Vector3 v in ctcduw.LoadedChunksToUnload)
            {
                RemoveChunk(v);
                AssignedWork.Remove(v);
            }

            int finalCount = prevCount - ChunkCollection.Count;

            if (finalCount > 0)
            {
                Debug.Log($"Unloaded {finalCount} chunks");
                Debug.Log($"There are currently {ChunkCollection.Count} loaded chunks");
            }
            
            NonChunkSpecificWork.Remove(JobType.UnloadOutsideViewDistance);
        } 

        public static void DrawAllChunksWithinViewDistance()
        {
            ThreadedChunkWork tcw;

            if (!NonChunkSpecificWork.TryGetValue(JobType.LoadViewDistance,out tcw))
            {
                ThreadedViewDistanceLoaderWork tvdlw = new ThreadedViewDistanceLoaderWork(ViewDistance, PlayerChunkPos, PlayerPositionHasChanged);
                NonChunkSpecificWork.Add(JobType.LoadViewDistance, tvdlw);
                ThreadHandler.EnqueuWork(tvdlw);
            }

            UnloadChunksOutsideViewDistance();
        }

        

        public static void LoadChunk(Vector3 chunkpos)
        {

        }

        public static void UnloadChunk(Vector3 chunkpos)
        {

        }

        public static void GenerateChunk(Vector3 chunkPos)
        {
            Chunk c;
            if (!ChunkCollection.TryGetValue(chunkPos, out c))
            {
                AssignedWorkContainer awc = GetWorkContainer(chunkPos);

                if (!awc.ContainsWork(JobType.Generate))
                {
                    ThreadedChunkGeneration tcg = new ThreadedChunkGeneration(chunkPos);
                    awc.AddAssignedJob(JobType.Generate, new AssignedJob(chunkPos, JobType.Generate, tcg));
                    ThreadHandler.EnqueuWork(tcg);
                }
            }
        }

        static AssignedWorkContainer GetWorkContainer(Vector3 chunkPos)
        {
            AssignedWorkContainer awc;
            AssignedWork.TryGetValue(chunkPos, out awc);

            if (awc == null)
            {
                awc = new AssignedWorkContainer();
                AssignedWork.Add(chunkPos, awc);
            }

            return awc;
        }

        public static void ReadCompletedThreadedChunkWork(CompletedThreadedChunkWork ctcw)
        {
            AssignedWorkContainer awc;
            
            switch (ctcw.WorkType)
            {
                case CompletedWorkType.Generate:
                    {
                        //add the chunk to the dict.
                        CompletedChunkGenerationWork ccgw = ctcw as CompletedChunkGenerationWork;
                        AssignedWork.TryGetValue(ccgw.Chunk.ChunkPos, out awc);
                        awc.RemoveAssignedJob(JobType.Generate);

                        ChunkCollection.TryAdd(ccgw.Chunk.ChunkPos, ccgw.Chunk);
                        ccgw.Chunk.SetGenerated();
                        break;
                    }
                case CompletedWorkType.ValidateDrawing:
                    {
                        CompletedChunkPendingDrawingWork ccpdw = ctcw as CompletedChunkPendingDrawingWork;

                        AssignedWork.TryGetValue(ccpdw.ChunkPos, out awc);
                        awc.RemoveAssignedJob(JobType.Draw);

                        if (ccpdw.CanDraw)
                        {
                            DrawChunk(ccpdw.ChunkPos);
                        } else
                        {
                            foreach (Vector3 pos in ccpdw.ChunksToGenerate)
                            {
                                GenerateChunk(pos);
                            }

                            TryDrawingChunk(ccpdw.ChunkPos);
                        }
                        break;
                    }
                case CompletedWorkType.Draw:
                    {
                        CheckCompletedDrawingWork(ctcw as CompletedSubChunkDrawingWork);
                        break;
                    }
                case CompletedWorkType.LoadViewDistance:
                    {
                        CheckCompletedViewDistanceLoaderWork(ctcw as CompletedViewDistanceLoaderWork);
                        break;
                    }
                case CompletedWorkType.UnloadOutsideViewDistance:
                    {
                        CheckCompletedViewDistanceUnloaderWork(ctcw as CompletedThreadedViewDistanceUnloader);
                        break;
                    }
            }
        }

        public static void UpdatePlayerPosition(Vector3 playerPos)
        {
            PlayerChunkPos = playerPos;
            PlayerPositionHasChanged = true;
            DrawAllChunksWithinViewDistance();
        }
    }
}
