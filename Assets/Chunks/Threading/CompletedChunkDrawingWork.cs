using Assets.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class CompletedChunkDrawingWork : CompletedThreadedChunkWork
    {
        MeshData meshdat;
        Vector3 chunkPos;

        public Vector3 ChunkPos
        {
            get
            {
                return chunkPos;
            }
        }

        public MeshData Meshdata
        {
            get
            {
                return meshdat;
            }
        }

        public CompletedChunkDrawingWork(MeshData md,Vector3 chunkPos, bool isValid) : base(CompletedWorkType.Draw,isValid)
        {
            meshdat = md;
            this.chunkPos = chunkPos;
        }

        public override void Dispose()
        {
            meshdat = null;
        }
    }
}
