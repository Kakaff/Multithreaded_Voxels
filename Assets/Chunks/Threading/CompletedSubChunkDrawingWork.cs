using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class CompletedSubChunkDrawingWork : CompletedThreadedChunkWork
    {
        MeshData meshdat;
        Vector3 chunkPos, subChunkPos;

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

        public Vector3 SubChunkPos
        {
            get
            {
                return subChunkPos;
            }
        }

        public CompletedSubChunkDrawingWork(MeshData md, Vector3 chunkPos, Vector3 subChunkPos, bool isValid) : base(CompletedWorkType.Draw,isValid)
        {
            meshdat = md;
            this.chunkPos = chunkPos;
            this.subChunkPos = subChunkPos;
        }

        public override void Dispose()
        {
            meshdat = null;
        }
    }
}
