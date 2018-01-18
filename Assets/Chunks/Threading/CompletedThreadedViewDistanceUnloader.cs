using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class CompletedThreadedViewDistanceUnloader : CompletedThreadedChunkWork
    {
        Vector3[] drawnChunksToUnload;
        Vector3[] loadedChunksToUnload;

        public Vector3[] DrawnChunksToUnload
        {
            get
            {
                return drawnChunksToUnload;
            }
        }

        public Vector3[] LoadedChunksToUnload
        {
            get
            {
                return loadedChunksToUnload;
            }
        }

        public CompletedThreadedViewDistanceUnloader(Vector3[] drawnChunksToUnload,Vector3[] loadedChunksToUnload, bool isValid) : base(CompletedWorkType.UnloadOutsideViewDistance,isValid)
        {
            this.drawnChunksToUnload = drawnChunksToUnload;
            this.loadedChunksToUnload = loadedChunksToUnload;
        }

        public override void Dispose()
        {
            drawnChunksToUnload = null;
            loadedChunksToUnload = null;
        }
    }
}
