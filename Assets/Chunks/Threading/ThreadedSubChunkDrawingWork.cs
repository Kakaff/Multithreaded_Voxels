using Assets.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class ThreadedSubChunkDrawingWork : ThreadedChunkWork
    {
        ChunkSubChunk csc;
        Vector3 chunkPos;

        public ThreadedSubChunkDrawingWork(ChunkSubChunk csc,Vector3 chunkPos) : base()
        {
            this.chunkPos = chunkPos;
            this.csc = csc;
        }

        public override CompletedThreadedWork Work()
        {

            return new CompletedSubChunkDrawingWork(csc.Draw(), chunkPos,csc.Pos, Isvalid);
        }
    }
}
