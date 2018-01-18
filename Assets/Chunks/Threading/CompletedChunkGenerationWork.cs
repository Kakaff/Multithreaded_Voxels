using Assets.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Chunks.Threading
{
    public class CompletedChunkGenerationWork : CompletedThreadedChunkWork
    {
        Chunk c;

        public Chunk Chunk
        {
            get
            {
                return c;
            }
        }

        public CompletedChunkGenerationWork(Chunk c,bool isValid) : base(CompletedWorkType.Generate,isValid)
        {
            this.c = c;
        }

        public override void Dispose()
        {
            c = null;
        }
    }
}
