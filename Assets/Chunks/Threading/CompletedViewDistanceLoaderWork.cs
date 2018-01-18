using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class CompletedViewDistanceLoaderWork : CompletedThreadedChunkWork
    {

        Vector3[] chunksToDraw;
        Vector3[] chunksToGenerate;

        public Vector3[] ChunksToDraw
        {
            get
            {
                return chunksToDraw;
            }
        }

        public Vector3[] ChunksToGenerate
        {
            get
            {
                return chunksToGenerate;
            }
        }

        public CompletedViewDistanceLoaderWork(Vector3[] chunksToDraw, Vector3[] chunksToGenerate, bool isValid) : base(CompletedWorkType.LoadViewDistance,isValid)
        {
            this.chunksToDraw = chunksToDraw;
            this.chunksToGenerate = chunksToGenerate;
        }

        public override void Dispose()
        {
            chunksToDraw = null;
            chunksToGenerate = null;
        }
    }
}
