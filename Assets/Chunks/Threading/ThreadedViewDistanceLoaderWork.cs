using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Threading;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class ThreadedViewDistanceLoaderWork : ThreadedChunkWork
    {
        int viewDist;
        int rawViewDist;
        bool hasMoved;
        Vector3 pChunkPos;
        static Dictionary<Vector3, DiscoveredChunk> cachedChunks = new Dictionary<Vector3, DiscoveredChunk>();
        static List<Vector3> cachedChunksToCheck = new List<Vector3>();

        public ThreadedViewDistanceLoaderWork(int viewDistance, Vector3 playerChunkPos, bool hasMoved) : base()
        {
            rawViewDist = viewDistance;
            pChunkPos = playerChunkPos;
            viewDist = viewDistance * viewDistance;
            this.hasMoved = hasMoved;

        }

        public override CompletedThreadedWork Work()
        {
            Queue<ChunkToCheck> chunksToCheck = new Queue<ChunkToCheck>();
            Dictionary<Vector3, DiscoveredChunk> DiscoveredChunks = new Dictionary<Vector3, DiscoveredChunk>();

            //Update the player chunk pos since it may have change since this was enqueued.
            pChunkPos = ChunkHandler.PlayerPosition;

            #region CacheStuff
            
            //Add previously cached chunks to be checked again (Chunks that needed to be generated).
            foreach (Vector3 v3 in cachedChunksToCheck)
            {
                int cost = CalcCost(v3);

                if (cost < viewDist)
                {
                    chunksToCheck.Enqueue(new ChunkToCheck(v3, cost));
                }
            }

            cachedChunksToCheck.Clear();


            #region Clean Cached DiscoveredChunks
            List<Vector3> cachedChunksToRemove = new List<Vector3>();
            //Clean up the cached DiscoveredChunks (Chunks that were drawn the last time we ran this check).
            if (hasMoved)
            {
                int altViewDist = (int)((rawViewDist - 2) * (rawViewDist - 2));
                foreach (DiscoveredChunk dc in cachedChunks.Values)
                {
                    //Check if the cached chunk is within viewdistance, if it isn't remove it.
                    int cost = CalcCost(dc.Position);

                    if (cost > viewDist)
                        cachedChunksToRemove.Add(dc.Position);
                    else if (cost > altViewDist) //Grabs the chunks at the edge of the viewdistance so that we can quickly find which chunks to load in when the player has moved.
                    {
                        chunksToCheck.Enqueue(new ChunkToCheck(dc.Position, cost));
                    }
                }

                //Remove the cached chunks that are now out of viewdistance.
                foreach (Vector3 v3 in cachedChunksToRemove)
                    cachedChunks.Remove(v3);
            }
            #endregion
            #endregion

            Chunk c;
            ChunkHandler.TryGetChunk(pChunkPos, out c);

            if (c != null && c.IsGenerated) {

                if (chunksToCheck.Count == 0)
                {
                    DiscoveredChunks.Add(pChunkPos, new DiscoveredChunk(pChunkPos, DiscoveredChunk.DiscoveryResult.Draw));

                    if (!c.IsSolid(Direction.East))
                        chunksToCheck.Enqueue(new ChunkToCheck(pChunkPos + new Vector3(1, 0, 0), 1));

                    if (!c.IsSolid(Direction.West))
                        chunksToCheck.Enqueue(new ChunkToCheck(pChunkPos + new Vector3(-1, 0, 0), 1));

                    if (!c.IsSolid(Direction.North))
                        chunksToCheck.Enqueue(new ChunkToCheck(pChunkPos + new Vector3(0, 0, 1), 1));

                    if (!c.IsSolid(Direction.South))
                        chunksToCheck.Enqueue(new ChunkToCheck(pChunkPos + new Vector3(0, 0, -1), 1));
                }

                ChunkToCheck current;
                Chunk currChunk;
                Vector3 vec;
                bool cachePos;
                //This is really slow, we could probably try and cache the discovered chunks that we should draw though and enqueue them for checking aswell.
                //So that we don't need to check them all over again unless the player has moved.

                while (true)
                {

                    cachePos = false;
                    //Try to fetch from cached chunks to check first.
                    if (chunksToCheck.Count == 0)
                    {
                        break;
                    }

                    current = chunksToCheck.Dequeue();
                    
                    ChunkHandler.TryGetChunk(current.Position, out currChunk);

                    if (currChunk == null)
                    {
                        DiscoveredChunks.Add(current.Position, new DiscoveredChunk(current.Position, DiscoveredChunk.DiscoveryResult.Generate));
                        continue;
                    }

                    if (!DiscoveredChunks.Keys.Contains(current.Position))
                    {
                        DiscoveredChunks.Add(current.Position, new DiscoveredChunk(current.Position, DiscoveredChunk.DiscoveryResult.Draw));
                    }

                    if (current.Cost < viewDist)
                    {
                        vec = current.Position + new Vector3(1, 0, 0);
                        DiscoveredChunk dc;
                        //Try to find x+1
                        if (!currChunk.IsSolid(Direction.East) && !DiscoveredChunks.TryGetValue(vec,out dc) && !cachedChunks.TryGetValue(vec,out dc))
                        {
                            ChunkHandler.TryGetChunk(vec, out c);
                            if (c != null && c.IsGenerated)
                            {
                                chunksToCheck.Enqueue(new ChunkToCheck(vec, CalcCost(vec)));
                            } else
                            {
                                cachePos = true;
                                DiscoveredChunks.Add(vec, new DiscoveredChunk(vec, DiscoveredChunk.DiscoveryResult.Generate));
                            }
                        }

                        vec = current.Position + new Vector3(-1, 0, 0);
                        //Try to find x-1
                        if (!currChunk.IsSolid(Direction.West) && !DiscoveredChunks.TryGetValue(vec,out dc) && !cachedChunks.TryGetValue(vec, out dc))
                        {
                            ChunkHandler.TryGetChunk(vec, out c);
                            if (c != null && c.IsGenerated)
                            {
                                chunksToCheck.Enqueue(new ChunkToCheck(vec, CalcCost(vec)));
                            }
                            else
                            {
                                cachePos = true;
                                DiscoveredChunks.Add(vec, new DiscoveredChunk(vec, DiscoveredChunk.DiscoveryResult.Generate));
                            }
                        }

                        vec = current.Position + new Vector3(0, 0, 1);
                        //Try to find z+1
                        if (!currChunk.IsSolid(Direction.North) && !DiscoveredChunks.TryGetValue(vec, out dc) && !cachedChunks.TryGetValue(vec, out dc))
                        {
                            ChunkHandler.TryGetChunk(vec, out c);
                            if (c != null && c.IsGenerated)
                            {
                                chunksToCheck.Enqueue(new ChunkToCheck(vec, CalcCost(vec)));
                            }
                            else
                            {
                                cachePos = true;
                                DiscoveredChunks.Add(vec, new DiscoveredChunk(vec, DiscoveredChunk.DiscoveryResult.Generate));
                            }
                        }
                        vec = current.Position + new Vector3(0, 0, -1);
                        //Try to find z-1
                        if (!currChunk.IsSolid(Direction.South) && !DiscoveredChunks.TryGetValue(vec, out dc) && !cachedChunks.TryGetValue(vec,out dc))
                        {
                            ChunkHandler.TryGetChunk(vec, out c);
                            if (c != null && c.IsGenerated)
                            {
                                chunksToCheck.Enqueue(new ChunkToCheck(vec, CalcCost(vec)));
                            }
                            else
                            {
                                cachePos = true;
                                DiscoveredChunks.Add(vec, new DiscoveredChunk(vec, DiscoveredChunk.DiscoveryResult.Generate));
                            }
                        }
                    }

                    if (cachePos)
                        cachedChunksToCheck.Add(current.Position);
                    //Check if we can reach chunk x+1, if we can check if the cost of it is within viewdist.
                }
            } else
            {
                //The chunk at the players position was null.
                DiscoveredChunks.Add(pChunkPos, new DiscoveredChunk(pChunkPos, DiscoveredChunk.DiscoveryResult.Generate));
            }

            #region SortResults
            List<Vector3> chunksToDraw = new List<Vector3>();
            List<Vector3> chunksToGenerate = new List<Vector3>();


            //ReEnqueue chunks that need to be generated to be checked.
            DiscoveredChunk dcTemp;
            foreach (DiscoveredChunk dc in DiscoveredChunks.Values)
            {
                if (dc.Result == DiscoveredChunk.DiscoveryResult.Generate)
                {
                    chunksToGenerate.Add(dc.Position);
                }
                else if (dc.Result == DiscoveredChunk.DiscoveryResult.Draw)
                {
                    chunksToDraw.Add(dc.Position);
                    if (!cachedChunks.TryGetValue(dc.Position,out dcTemp))
                        cachedChunks.Add(dc.Position, dc);
                }
            }

            #endregion
            //Make life easier for the GC.
            DiscoveredChunks.Clear();
            DiscoveredChunks = null;

            //Sort which chunks to generate and which ones to draw.
            //If the number of chunks to generate is greater than one, flag this for being done again.
            return new CompletedViewDistanceLoaderWork(chunksToDraw.ToArray(), chunksToGenerate.ToArray(),Isvalid);
        }

        int CalcCost(Vector3 pos)
        {
            Vector3 nPos = pos - pChunkPos;

            float x = nPos.x * nPos.x;
            float y = nPos.y * nPos.y;
            float z = nPos.z * nPos.z;

            return (int)(x + y + z);
        }
    }


    public class DiscoveredChunk
    {
        public enum DiscoveryResult
        {
            Generate,
            Draw
        }

        public DiscoveryResult Result
        {
            get
            {
                return res;
            }
        }

        public Vector3 Position
        {
            get
            {
                return chunkPos;
            }
        }

        DiscoveryResult res;
        Vector3 chunkPos;

        public DiscoveredChunk(Vector3 cPos, DiscoveryResult res)
        {
            this.res = res;
            chunkPos = cPos;
        }
    }

    public class ChunkToCheck
    {
        int cost;
        Vector3 pos;

        public int Cost
        {
            get
            {
                return cost;
            }
        }

        public Vector3 Position
        {
            get
            {
                return pos;
            }
        }

        public ChunkToCheck(Vector3 pos, int cost)
        {
            this.cost = cost;
            this.pos = pos;
        }
    }
}
