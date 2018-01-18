using Assets.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class ThreadedPendingDrawingWork : ThreadedChunkWork
    {
        Vector3 chunkPos;

        public ThreadedPendingDrawingWork(Vector3 chunkPos) : base()
        {
            this.chunkPos = chunkPos;
        }

        public override CompletedThreadedWork Work()
        {
            Chunk c;
            Chunk mc;
            ChunkHandler.TryGetChunk(chunkPos, out mc);
            Chunk[,,] neighbors = new Chunk[3, 3, 3];
            List<Vector3> chunksThatNeedGeneratíng = new List<Vector3>();


            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    for (int z = -1; z < 2; z++)
                    {
                        if (!ChunkHandler.TryGetChunk(chunkPos + new Vector3(x,y,z),out neighbors[x+1,y+1,z+1]))
                        {
                            chunksThatNeedGeneratíng.Add(chunkPos + new Vector3(x, y, z));
                        }
                    }
                }
            }
            
            if (chunksThatNeedGeneratíng.Count == 0 && !mc.HasSurroundingBlocks)
            {
                //Get the surrounding blocks if we haven't already.
                //This is a dirty fix so that we can use spline smoothing in the world generator.
                for (int x = 0; x < Chunk.CHUNKSIZE; x++)
                {
                    for (int z = 0; z < Chunk.CHUNKSIZE; z++)
                    {
                        //East + west
                        mc.SetBlockUnsafe(0, z + 1, x + 1, neighbors[0, 1, 1].GetBlockIDUnsafe(Chunk.CHUNKSIZE, z + 1, x + 1));
                        mc.SetBlockUnsafe(Chunk.CHUNKSIZE +1, z + 1, x + 1, neighbors[2, 1, 1].GetBlockIDUnsafe(1, z + 1, x + 1));

                        mc.SetShapeUnsafe(0, z + 1, x + 1, neighbors[0, 1, 1].GetShapeIDUnsafe(Chunk.CHUNKSIZE, z + 1, x + 1));
                        mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, z + 1, x + 1, neighbors[2, 1, 1].GetShapeIDUnsafe(1, z + 1, x + 1));

                        //North + south
                        mc.SetBlockUnsafe(x + 1, z + 1, 0, neighbors[1, 1, 0].GetBlockIDUnsafe(x + 1, z + 1, Chunk.CHUNKSIZE));
                        mc.SetBlockUnsafe(x + 1, z + 1, Chunk.CHUNKSIZE + 1, neighbors[1, 1, 2].GetBlockIDUnsafe(x + 1, z + 1, 1));

                        mc.SetShapeUnsafe(x + 1, z + 1, 0, neighbors[1, 1, 0].GetShapeIDUnsafe(x + 1, z + 1, Chunk.CHUNKSIZE));
                        mc.SetShapeUnsafe(x + 1, z + 1, Chunk.CHUNKSIZE + 1, neighbors[1, 1, 2].GetShapeIDUnsafe(x + 1, z + 1, 1));

                        //Up + Down
                        mc.SetBlockUnsafe(x + 1, Chunk.CHUNKSIZE + 1, z + 1, neighbors[1, 2, 1].GetBlockIDUnsafe(x + 1, 1, z + 1));
                        mc.SetBlockUnsafe(x + 1, 0, z + 1, neighbors[1, 0, 1].GetBlockIDUnsafe(x + 1, Chunk.CHUNKSIZE, z + 1));

                        mc.SetShapeUnsafe(x + 1, Chunk.CHUNKSIZE + 1, z + 1, neighbors[1, 2, 1].GetShapeIDUnsafe(x + 1, 1, z + 1));
                        mc.SetShapeUnsafe(x + 1, 0, z + 1, neighbors[1, 0, 1].GetShapeIDUnsafe(x + 1, Chunk.CHUNKSIZE, z + 1));
                    }

                    //Set corner blocks & shapes

                    //Up West + Up East
                    mc.SetBlockUnsafe(0, Chunk.CHUNKSIZE +1, x + 1, neighbors[0, 2, 1].GetBlockIDUnsafe(Chunk.CHUNKSIZE, 1, x + 1)); //Up + West
                    mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, x + 1, neighbors[2, 2, 1].GetBlockIDUnsafe(1, 1, x + 1)); //Up + East

                    mc.SetShapeUnsafe(0, Chunk.CHUNKSIZE + 1, x + 1, neighbors[0, 2, 1].GetShapeIDUnsafe(Chunk.CHUNKSIZE, 1, x + 1)); //Up + West
                    mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, x + 1, neighbors[2, 2, 1].GetShapeIDUnsafe(1, 1, x + 1)); //Up + East

                    //Up North + Up South
                    mc.SetBlockUnsafe(x + 1, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[1, 2, 2].GetBlockIDUnsafe(x + 1, 1, 1)); //Up + North
                    mc.SetBlockUnsafe(x + 1, Chunk.CHUNKSIZE + 1, 0, neighbors[1, 2, 0].GetBlockIDUnsafe(x + 1, 1, Chunk.CHUNKSIZE)); //Up + South

                    mc.SetShapeUnsafe(x + 1, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[1, 2, 2].GetShapeIDUnsafe(x + 1, 1, 1)); //Up + North
                    mc.SetShapeUnsafe(x + 1, Chunk.CHUNKSIZE + 1, 0, neighbors[1, 2, 0].GetShapeIDUnsafe(x + 1, 1, Chunk.CHUNKSIZE)); //Up + South

                    //Down West + Down East
                    mc.SetBlockUnsafe(0, 0, x + 1, neighbors[0, 0, 1].GetBlockIDUnsafe(Chunk.CHUNKSIZE, Chunk.CHUNKSIZE, x + 1)); //Down + West
                    mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, 0, x + 1, neighbors[2, 0, 1].GetBlockIDUnsafe(1, Chunk.CHUNKSIZE, x + 1)); //Down + East

                    mc.SetShapeUnsafe(0, 0, x + 1, neighbors[0, 0, 1].GetShapeIDUnsafe(Chunk.CHUNKSIZE, Chunk.CHUNKSIZE, x + 1)); //Down + West
                    mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, 0, x + 1, neighbors[2, 0, 1].GetShapeIDUnsafe(1, Chunk.CHUNKSIZE, x + 1)); //Down + East

                    //Down North + Down South
                    mc.SetBlockUnsafe(x + 1, 0, Chunk.CHUNKSIZE + 1, neighbors[1, 0, 2].GetBlockIDUnsafe(x + 1, Chunk.CHUNKSIZE, 1)); //Down + North
                    mc.SetBlockUnsafe(x + 1, 0, 0, neighbors[1, 0, 0].GetBlockIDUnsafe(x + 1, Chunk.CHUNKSIZE, Chunk.CHUNKSIZE)); //Down + South

                    mc.SetShapeUnsafe(x + 1, 0, Chunk.CHUNKSIZE + 1, neighbors[1, 0, 2].GetShapeIDUnsafe(x + 1, Chunk.CHUNKSIZE, 1)); //Down + North
                    mc.SetShapeUnsafe(x + 1, 0, 0, neighbors[1, 0, 0].GetShapeIDUnsafe(x + 1, Chunk.CHUNKSIZE, Chunk.CHUNKSIZE)); //Down + South

                    //North East, North West
                    mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, x + 1, Chunk.CHUNKSIZE + 1, neighbors[2, 1, 2].GetBlockIDUnsafe(1, x + 1, 1)); //North + East
                    mc.SetBlockUnsafe(0, x + 1, Chunk.CHUNKSIZE + 1, neighbors[0, 1, 2].GetBlockIDUnsafe(Chunk.CHUNKSIZE, x + 1, 1)); //North + West

                    mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, x + 1, Chunk.CHUNKSIZE + 1, neighbors[2, 1, 2].GetShapeIDUnsafe(1, x + 1, 1)); //North + East
                    mc.SetShapeUnsafe(0, x + 1, Chunk.CHUNKSIZE + 1, neighbors[0, 1, 2].GetShapeIDUnsafe(Chunk.CHUNKSIZE, x + 1, 1)); //North + West
                    //South East, South West
                    mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, x + 1, 0, neighbors[2, 1, 0].GetBlockIDUnsafe(1, x + 1, Chunk.CHUNKSIZE)); //South + East
                    mc.SetBlockUnsafe(0, x + 1, 0, neighbors[0, 1, 0].GetBlockIDUnsafe(Chunk.CHUNKSIZE, x + 1, Chunk.CHUNKSIZE)); //South + West

                    mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, x + 1, 0, neighbors[2, 1, 0].GetShapeIDUnsafe(1, x + 1, Chunk.CHUNKSIZE)); //South + East
                    mc.SetShapeUnsafe(0, x + 1, 0, neighbors[0, 1, 0].GetShapeIDUnsafe(Chunk.CHUNKSIZE, x + 1, Chunk.CHUNKSIZE)); //South + West
                }

                //Set corner blocks, UpNorthEast, UpNorthWest, UpSouthEast, UpSouthWest

                mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[2, 2, 2].GetBlockIDUnsafe(1, 1, 1)); //Up North East
                mc.SetBlockUnsafe(0, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[0, 2, 2].GetBlockIDUnsafe(Chunk.CHUNKSIZE, 1, 1)); //Up North West
                mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, 0, neighbors[2, 2, 0].GetBlockIDUnsafe(1, 1, Chunk.CHUNKSIZE)); //Up South East
                mc.SetBlockUnsafe(0, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[0, 2, 0].GetBlockIDUnsafe(Chunk.CHUNKSIZE, 1, Chunk.CHUNKSIZE)); //Up South West

                mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, 0, Chunk.CHUNKSIZE + 1, neighbors[2, 0, 2].GetBlockIDUnsafe(1, Chunk.CHUNKSIZE, 1)); //Down North East
                mc.SetBlockUnsafe(0, 0, Chunk.CHUNKSIZE + 1, neighbors[0, 0, 2].GetBlockIDUnsafe(Chunk.CHUNKSIZE, Chunk.CHUNKSIZE, 1)); //Down North West
                mc.SetBlockUnsafe(Chunk.CHUNKSIZE + 1, 0, 0, neighbors[2, 0, 0].GetBlockIDUnsafe(1, Chunk.CHUNKSIZE, Chunk.CHUNKSIZE)); //Down South East
                mc.SetBlockUnsafe(0, 0, Chunk.CHUNKSIZE + 1, neighbors[0, 0, 0].GetBlockIDUnsafe(Chunk.CHUNKSIZE, Chunk.CHUNKSIZE, Chunk.CHUNKSIZE)); //Down South West

                mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[2, 2, 2].GetShapeIDUnsafe(1, 1, 1)); //Up North East
                mc.SetShapeUnsafe(0, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[0, 2, 2].GetShapeIDUnsafe(Chunk.CHUNKSIZE, 1, 1)); //Up North West
                mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, 0, neighbors[2, 2, 0].GetShapeIDUnsafe(1, 1, Chunk.CHUNKSIZE)); //Up South East
                mc.SetShapeUnsafe(0, Chunk.CHUNKSIZE + 1, Chunk.CHUNKSIZE + 1, neighbors[0, 2, 0].GetShapeIDUnsafe(Chunk.CHUNKSIZE, 1, Chunk.CHUNKSIZE)); //Up South West

                mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, 0, Chunk.CHUNKSIZE + 1, neighbors[2, 0, 2].GetShapeIDUnsafe(1, Chunk.CHUNKSIZE, 1)); //Down North East
                mc.SetShapeUnsafe(0, 0, Chunk.CHUNKSIZE + 1, neighbors[0, 0, 2].GetShapeIDUnsafe(Chunk.CHUNKSIZE, Chunk.CHUNKSIZE, 1)); //Down North West
                mc.SetShapeUnsafe(Chunk.CHUNKSIZE + 1, 0, 0, neighbors[2, 0, 0].GetShapeIDUnsafe(1, Chunk.CHUNKSIZE, Chunk.CHUNKSIZE)); //Down South East
                mc.SetShapeUnsafe(0, 0, Chunk.CHUNKSIZE + 1, neighbors[0, 0, 0].GetShapeIDUnsafe(Chunk.CHUNKSIZE, Chunk.CHUNKSIZE, Chunk.CHUNKSIZE)); //Down South West
            }

            c = null;
            mc = null;
            neighbors = null;

            return new CompletedChunkPendingDrawingWork(chunkPos, chunksThatNeedGeneratíng.ToArray(), chunksThatNeedGeneratíng.Count == 0, Isvalid);
        }
    }
}
