using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Threading
{
    public class WorkBuffer
    {
        public static int MaxBufferSize = 5;
        ThreadedWork[] jobs;

        public ThreadedWork[] Jobs
        {
            get
            {
                return jobs;
            }
        }

        public WorkBuffer(params ThreadedWork[] tw)
        {
            jobs = tw;
        }

        public void Dispose()
        {
            jobs = null;
        }
    }
}
