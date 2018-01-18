using Assets.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Chunks.Threading
{
    
    public enum CompletedWorkType
    {
        Generate,
        ValidateDrawing,
        LoadViewDistance,
        UnloadOutsideViewDistance,
        Draw,
    }

    public class CompletedThreadedChunkWork : CompletedThreadedWork
    {
        CompletedWorkType identifier;

        public CompletedWorkType WorkType
        {
            get
            {
                return identifier;
            }
        }

        protected CompletedThreadedChunkWork(CompletedWorkType identifier,bool isValid) : base(2,isValid)
        {
            this.identifier = identifier;
        }
    }
}
