using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Chunks.Threading
{
    public class AssignedWorkContainer
    {
        Dictionary<JobType, AssignedJob> assignedWork = new Dictionary<JobType, AssignedJob>();

        public bool ContainsWork(JobType jp)
        {
            AssignedJob aj;
            return assignedWork.TryGetValue(jp, out aj);
        }

        public void AddAssignedJob(JobType jp, AssignedJob aj)
        {
            assignedWork.Add(jp, aj);
        }

        public void RemoveAssignedJob(JobType jp)
        {
            AssignedJob aj;

            assignedWork.TryGetValue(jp, out aj);
            if (aj != null)
            {
                assignedWork.Remove(jp);
                aj.Dispose();
            }
        }

        public void InvalidateWork(JobType jp)
        {
            AssignedJob aj;
            if (assignedWork.TryGetValue(jp, out aj))
                foreach (ThreadedChunkWork tcw in aj.WorkArray)
                    tcw.FlagInvalid();
        }

        public AssignedJob TryGetAssignedJob(JobType jp)
        {
            AssignedJob aj;
            assignedWork.TryGetValue(jp, out aj);

            return aj;
        }
    }
}
