using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Threading;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class ThreadedViewDistanceUnloaderWork : ThreadedChunkWork
    {

        Vector3[] loadedChunks;
        Vector3[] drawnChunks;
        Vector3 pChunkPos;
        int viewDist;

        public ThreadedViewDistanceUnloaderWork(Vector3 playerChunkPos,int ViewDistance,Vector3[] loadedChunks, Vector3[] drawnChunks)
        {
            this.loadedChunks = loadedChunks;
            this.drawnChunks = drawnChunks;
            pChunkPos = playerChunkPos;
            viewDist = ViewDistance;
        }

        public override CompletedThreadedWork Work()
        {
            List<Vector3> drawnChunkstToUnload = new List<Vector3>();
            pChunkPos = ChunkHandler.PlayerPosition;

            int drawnUnloadDist = (int)Mathf.Pow(viewDist + 1.5f, 2);
            //Filter which drawn chunks to unload.
            foreach (Vector3 v3 in drawnChunks)
            {
                if (CalcCost(v3) > drawnUnloadDist)
                    drawnChunkstToUnload.Add(v3);
            }

            //Filter which loaded but not drawn chunks to unload.
            List<Vector3> loadedChunksToUnload = new List<Vector3>();
            int loadedUnloadDist = (int)Mathf.Pow(viewDist + 3f, 2);
            foreach (Vector3 v3 in loadedChunks)
            {
                if (CalcCost(v3) > loadedUnloadDist)
                {
                    loadedChunksToUnload.Add(v3);
                }
            }

            return new CompletedThreadedViewDistanceUnloader(drawnChunkstToUnload.ToArray(), loadedChunksToUnload.ToArray(), Isvalid);
        }


        int CalcCost(Vector3 pos)
        {
            Vector3 nPos = pos - pChunkPos;
            float x = nPos.x * nPos.x;
            float y = nPos.y * nPos.y;
            float z = nPos.z * nPos.z;

            return (int)(x + y + z);
        }

        public override void Dispose()
        {
            loadedChunks = null;
            drawnChunks = null;
        }
    }
}
