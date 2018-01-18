using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Threading;

namespace Assets.Chunks.Threading
{
    public class ThreadedChunkDrawing : ThreadedChunkWork
    {
        Chunk c;

        public ThreadedChunkDrawing(Chunk c) : base()
        {
            this.c = c;
        }

        public override CompletedThreadedWork Work()
        {
            return new CompletedChunkDrawingWork(c.Draw(), c.ChunkPos,Isvalid);
        }

        public override void Dispose()
        {
            c = null;
        }
    }
}
