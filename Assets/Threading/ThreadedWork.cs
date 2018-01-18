using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Threading
{
    public class ThreadedWork
    {
        bool isValid;

        public bool Isvalid
        {
            get
            {
                return isValid;
            }
        }

        protected ThreadedWork()
        {
            isValid = true;
        }

        public virtual void Dispose()
        {

        }

        public virtual CompletedThreadedWork Work()
        {
            return null;
        }

        public void FlagInvalid()
        {
            isValid = false;
        }
    }
}
