using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Threading
{
    public class WorkerThread
    {
        List<WorkBuffer> WorkBuffer;
        Queue<WorkBuffer> PrivWorkBuffer;
        List<CompletedThreadedWork> PrivCompletedBuffer;
        List<CompletedThreadedWork> CompletedWork;

        object lockObject_CompletedWork;
        object lockObject_WorkBuffer;

        int id;
        bool isStarted;

        int targetWorkCycles = 5;

        public bool AllowMoveToPublicCompleteBuffer;
        public bool AllowMoveFromPublicWorkBuffer;

        Thread thread;

        int MaxCompletedWorkInPrivBuffer = 12;

        public WorkerThread(int id)
        {
            AllowMoveFromPublicWorkBuffer = true;
            AllowMoveToPublicCompleteBuffer = true;

            this.id = id;
            isStarted = false;
            lockObject_CompletedWork = new object();
            lockObject_WorkBuffer = new object();
            thread = new Thread(Loop);
            WorkBuffer = new List<WorkBuffer>();
            PrivWorkBuffer = new Queue<WorkBuffer>();
            PrivCompletedBuffer = new List<CompletedThreadedWork>();
            CompletedWork = new List<CompletedThreadedWork>();
        }


        public void AddWork(WorkBuffer[] work)
        {
            if (work != null)
                WorkBuffer.AddRange(work);
        }

        public void Start()
        {
            if (!isStarted)
            {
                thread.Start();
                isStarted = true;
            }
        }

        public bool TryLockWorkBuffer(int timeout)
        {
            return Monitor.TryEnter(lockObject_WorkBuffer, timeout);
        }

        public void LockWorkBuffer()
        {
            Monitor.Enter(lockObject_WorkBuffer);
        }

        public void ReleaseWorkBuffer()
        {
            Monitor.Exit(lockObject_WorkBuffer);
        }

        public bool TryLockCompletedWorkBuffer(int timeout)
        {
            return Monitor.TryEnter(lockObject_CompletedWork, timeout);
        }

        public void LockCompletedWorkBuffer()
        {
            Monitor.Enter(lockObject_CompletedWork);
        }

        public void ReleaseCompletedWorkBuffer()
        {
            Monitor.Exit(lockObject_CompletedWork);
        }

        public CompletedThreadedWork[] GetCompletedWork()
        {
            CompletedThreadedWork[] ctw = CompletedWork.ToArray();
            CompletedWork.Clear();

            return ctw;
        }

        void GetWorkFromThreadHandler(int lockTimeout,int sleepTime)
        {

            if (ThreadHandler.AllowWorkerThreadsToCollectWork && ThreadHandler.TryGetLock(lockTimeout))
            {
                ThreadHandler.GetWork();
                ThreadHandler.ReleaseLock();
            }
            else
            {
                Thread.Sleep(sleepTime);
            }
        }

        void MoveCompletedWorkFromPrivateToPublic(int lockTimeOut)
        {
            if (AllowMoveToPublicCompleteBuffer)
            {
                bool cbLock = false;

                if (PrivCompletedBuffer.Count > 0 && (cbLock = TryLockCompletedWorkBuffer(lockTimeOut)))
                {
                    CompletedWork.AddRange(PrivCompletedBuffer.ToArray());
                    PrivCompletedBuffer.Clear();
                    ReleaseCompletedWorkBuffer();
                } /*else if (PrivCompletedBuffer.Count > MaxCompletedWorkInPrivBuffer)
                {
                    LockCompletedWorkBuffer();
                    CompletedWork.AddRange(PrivCompletedBuffer.ToArray());
                    PrivCompletedBuffer.Clear();
                    ReleaseCompletedWorkBuffer();
                } */
            }
        }

        void PerformWork()
        {
            if (PrivWorkBuffer.Count > 0)
            {
                int numWork = (PrivWorkBuffer.Count > targetWorkCycles) ? targetWorkCycles : PrivWorkBuffer.Count;

                for (int i = 0; i < numWork; i++)
                {
                    WorkBuffer wb = PrivWorkBuffer.Dequeue();

                    foreach (ThreadedWork tw in wb.Jobs)
                    {
                        PrivCompletedBuffer.Add(tw.Work());
                        tw.Dispose();
                    }
                }
            }
        }

        void MoveWorkFromPublicToPrivateBuffer(int lockTimeout)
        {
            bool wbLock = false;

            if (AllowMoveFromPublicWorkBuffer && WorkBuffer.Count > 0 && (wbLock = TryLockWorkBuffer(lockTimeout)))
            {
                foreach (WorkBuffer wb in WorkBuffer)
                    PrivWorkBuffer.Enqueue(wb);

                ReleaseWorkBuffer();
                WorkBuffer.Clear();
            }
            else if (!wbLock && PrivWorkBuffer.Count == 0)
            {
                MoveCompletedWorkFromPrivateToPublic(5);
            }
            else if (WorkBuffer.Count == 0 && PrivWorkBuffer.Count == 0)
            {
                GetWorkFromThreadHandler(5, 5);
            }
        }

        void Loop()
        {
            while (true)
            {

                MoveWorkFromPublicToPrivateBuffer(2);

                PerformWork();

                GetWorkFromThreadHandler(3, 5);

                MoveCompletedWorkFromPrivateToPublic(2);
            }

        }
    }
}
