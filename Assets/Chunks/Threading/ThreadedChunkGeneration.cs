using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Threading;
using UnityEngine;
using Assets.Blocks;

namespace Assets.Chunks.Threading
{

    


    public class ThreadedChunkGeneration : ThreadedChunkWork
    {
        Vector3 chunkPos;

        public ThreadedChunkGeneration(Vector3 ChunkPos) : base()
        {
            chunkPos = ChunkPos;
        }

        public override CompletedThreadedWork Work()
        {

            return InterpolatedResult();
        }

        public CompletedChunkGenerationWork InterpolatedResult()
        {
            Chunk c = new Chunk(chunkPos);

            float modScale = 0.025f;
            int heightMod = 14;

            
            float x0z0, x1z0, x2z0, x3z0, x4z0;
            float x0z1, x1z1, x2z1, x3z1, x4z1;
            float x0z2, x1z2, x2z2, x3z2, x4z2;
            float x0z3, x1z3, x2z3, x3z3, x4z3;
            float x0z4, x1z4, x2z4, x3z4, x4z4;

            x0z0 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 9) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 9) * modScale) * heightMod;
            x1z0 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 1) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 9) * modScale) * heightMod;
            x2z0 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 7) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 9) * modScale) * heightMod;
            x3z0 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 15) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 9) * modScale) * heightMod;
            x4z0 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 23) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 9) * modScale) * heightMod;

            x0z1 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 9) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 1) * modScale) * heightMod;
            x1z1 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 1) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 1) * modScale) * heightMod;
            x2z1 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 7) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 1) * modScale) * heightMod;
            x3z1 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 15) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 1) * modScale) * heightMod;
            x4z1 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 23) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) - 1) * modScale) * heightMod;

            x0z2 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 9) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 7) * modScale) * heightMod;
            x1z2 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 1) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 7) * modScale) * heightMod;
            x2z2 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 7) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 7) * modScale) * heightMod;
            x3z2 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 15) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 7) * modScale) * heightMod;
            x4z2 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 23) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 7) * modScale) * heightMod;

            x0z3 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 9) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 15) * modScale) * heightMod;
            x1z3 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 1) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 15) * modScale) * heightMod;
            x2z3 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 7) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 15) * modScale) * heightMod;
            x3z3 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 15) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 15) * modScale) * heightMod;
            x4z3 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 23) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 15) * modScale) * heightMod;

            x0z4 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 9) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 23) * modScale) * heightMod;
            x1z4 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) - 1) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 23) * modScale) * heightMod;
            x2z4 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 7) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 23) * modScale) * heightMod;
            x3z4 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 15) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 23) * modScale) * heightMod;
            x4z4 = Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + 23) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + 23) * modScale) * heightMod;

            PointField3x3 pFieldx0z0, pFieldx1z0, pFieldx0z1, pFieldx1z1;

            pFieldx0z0 = new PointField3x3(x0z0, x1z0, x2z0, 
                                           x0z1, x1z1, x2z1, 
                                           x0z2, x1z2, x2z2);

            pFieldx1z0 = new PointField3x3(x2z0, x3z0, x4z0,
                                           x2z1, x3z1, x4z1,
                                           x2z2, x3z2, x4z2);

            pFieldx0z1 = new PointField3x3(x0z2, x1z2, x2z2,
                                           x0z3, x1z3, x2z3,
                                           x0z4, x1z4, x2z4);

            pFieldx1z1 = new PointField3x3(x2z2, x3z2, x4z2,
                                           x2z3, x3z3, x4z3,
                                           x2z4, x3z4, x4z4);

            float heightValx0z0,heightValx1z0,heightValx0z1, heightValx1z1;

            for (int x = -1; x < 9; x++)
            {
                for (int z = -1; z < 9; z++)
                {
                    heightValx0z0 = pFieldx0z0.GetValue(x+10, z+10,18, InterpolationMethod.Spline);
                    heightValx1z0 = pFieldx1z0.GetValue(x + 1, z + 10, 18, InterpolationMethod.Spline);
                    heightValx0z1 = pFieldx0z1.GetValue(x + 10, z + 1, 18, InterpolationMethod.Spline);
                    heightValx1z1 = pFieldx1z1.GetValue(x + 1, z + 1, 18, InterpolationMethod.Spline);

                    for (int y = -1; y < Chunk.CHUNKSIZE+1; y++)
                    {
                        if (heightValx0z0 > (chunkPos.y * Chunk.CHUNKSIZE) + y)
                            c.SetBlockUnsafe(x + 1, y + 1, z + 1, 1);

                        if (heightValx1z0 > (chunkPos.y * Chunk.CHUNKSIZE) + y)
                            c.SetBlockUnsafe(x + 9, y + 1, z + 1, 1);

                        if (heightValx0z1 > (chunkPos.y * Chunk.CHUNKSIZE) + y)
                            c.SetBlockUnsafe(x + 1, y + 1, z + 9, 1);

                        if (heightValx1z1 > (chunkPos.y * Chunk.CHUNKSIZE) + y)
                            c.SetBlockUnsafe(x + 9, y + 1, z + 9, 1);
                    }
                }
            }

            c.CalculateShapes();
            c.CalculateEdgeSolidity();
            return new CompletedChunkGenerationWork(c, Isvalid);
        }
        
        public CompletedChunkGenerationWork NonInterpolatedResult()
        {
            Chunk c = new Chunk(chunkPos);

            int heightVal;
            float modScale = 0.025f;

            for (int x = -1; x < Chunk.CHUNKSIZE; x++)
            {
                for (int z = -1; z < Chunk.CHUNKSIZE; z++)
                {
                    heightVal = (int)(Mathf.PerlinNoise(((chunkPos.x * Chunk.CHUNKSIZE) + x) * modScale, ((chunkPos.z * Chunk.CHUNKSIZE) + z) * modScale) * 14);

                    for (int y = -1; y < Chunk.CHUNKSIZE; y++)
                    {
                       c.SetBlockUnsafe(x+1, y+1, z+1, ((c.ChunkPos.y * Chunk.CHUNKSIZE) + y < heightVal ? 1 : 0));
                       
                    }
                }
            }

            return new CompletedChunkGenerationWork(c, Isvalid);
        }

        
        static float BiLinearInterPol(float x0z0,float x1z0,float x0z1,float x1z1, float xT,float zT)
        {
            /*
            float l1 = Lerp(x0z0, x1z0, xT);
            float l2 = Lerp(x0z1, x1z1, xT);

            return Lerp(l1, l2, zT);
            */

            float l1 = Mathf.SmoothStep(x0z0, x1z0, xT);
            float l2 = Mathf.SmoothStep(x0z1, x1z1, xT);

            return Mathf.SmoothStep(l1, l2, zT);
            //Lerp between l1 and l2 along 
        }

        static float Lerp(float f1,float f2,float t)
        {
            return (f1 * (1f - t)) + (f2 * t);
        }
    }
}
