using Assets.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class CompletedChunkPendingDrawingWork : CompletedThreadedChunkWork
    {
        Vector3 chunkPos;
        bool canDraw;
        Vector3[] chunksToGenerate;

        public Vector3 ChunkPos
        {
            get
            {
                return chunkPos;
            }
        }

        public bool CanDraw
        {
            get
            {
                return canDraw;
            }
        }

        public Vector3[] ChunksToGenerate
        {
            get
            {
                return chunksToGenerate;
            }
        }

        public CompletedChunkPendingDrawingWork(Vector3 chunkPos,Vector3[] chunksToGenerate, bool canDraw,bool isValid) : base(CompletedWorkType.ValidateDrawing,isValid)
        {
            this.chunkPos = chunkPos;
            this.canDraw = canDraw;
            this.chunksToGenerate = chunksToGenerate;
        }

        public override void Dispose()
        {
            chunksToGenerate = null;
        }


    }
}
