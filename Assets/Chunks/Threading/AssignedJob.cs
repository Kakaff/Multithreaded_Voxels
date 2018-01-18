using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Chunks.Threading
{
    public enum JobType
    {
        Generate,
        Draw,
        LoadViewDistance,
        UnloadOutsideViewDistance,
    }

    public class AssignedJob
    {
        Vector3 chunkPos;
        JobType jp;
        ThreadedChunkWork[] work;
        int _completedCount;

        public Vector3 OwnerChunkPos
        {
            get
            {
                return chunkPos;
            }
        }

        public JobType JobType
        {
            get
            {
                return jp;
            }
        }

        public ThreadedChunkWork[] WorkArray
        {
            get
            {
                return work;
            }
        }

        public int CompletedCount
        {
            get
            {
                return _completedCount;
            }
            set
            {
                _completedCount = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return CompletedCount == WorkArray.Length;
            }
        }

        public AssignedJob(Vector3 chunkPos,JobType jp, params ThreadedChunkWork[] work)
        {
            this.work = work;
            this.jp = jp;
            this.chunkPos = chunkPos;
        }

        public void Dispose()
        {
            work = null;
        }
    }
}
