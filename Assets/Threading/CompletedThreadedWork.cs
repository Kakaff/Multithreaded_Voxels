using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Threading
{
    public class CompletedThreadedWork
    {
        int workIdentifier;
        bool isValid;

        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }

        public int Identifier
        {
            get
            {
                return workIdentifier;
            }
        }

        protected CompletedThreadedWork(int workIdentifier,bool isValid)
        {
            this.workIdentifier = workIdentifier;
        }

        public virtual void Dispose()
        {

        }
    }
}
