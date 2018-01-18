using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Threading;
using UnityEngine;

namespace Assets.Chunks.Threading
{
    public class ThreadedSubChunkGenerationWork : ThreadedChunkWork
    {

        Vector3 subChunkPos, chunkPos;

        public ThreadedSubChunkGenerationWork(Vector3 chunkPos,Vector3 subChunkPos) : base()
        {
            this.chunkPos = chunkPos;
            this.subChunkPos = subChunkPos;
        }

        public override CompletedThreadedWork Work()
        {
            //Return a completed work that contains the generated subchunk.
            return base.Work();
        }
    }
}
