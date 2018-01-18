using Assets.Chunks;
using Assets.Chunks.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Threading
{
    public static class ThreadHandler
    {

        static WorkerThread[] Threads;
        static List<WorkBuffer>[] WorkBuffers;
        static int[] FailedRetrieveAttempts; //How many times we've failed to get the completed work from a thread.
        static int MaxFailedRetrieveAttempts = 5;

        static int[] FailedWorkAssignments;
        static int MaxFailedWorkAssignments = 5;

        static List<ThreadedWork> workToBeAssigned = new List<ThreadedWork>();
        static WorkBuffer[] FailureBuffer;
        static List<CompletedThreadedWork> CompletedWorkBuffer = new List<CompletedThreadedWork>();

        static object lockObject_WorkBuffers = new object();

        public static bool AllowWorkerThreadsToCollectWork = true;


        public static void InitThreads(int threadCount)
        {
            Threads = new WorkerThread[threadCount];
            WorkBuffers = new List<WorkBuffer>[threadCount];
            FailedWorkAssignments = new int[threadCount];
            FailedRetrieveAttempts = new int[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                Threads[i] = new WorkerThread(i);
                WorkBuffers[i] = new List<WorkBuffer>();
            }
        }

        public static void StartThreads()
        {
            foreach (WorkerThread wt in Threads)
                wt.Start();
        }

        public static void AddWork(ThreadedWork[] tw)
        {
            throw new System.Exception("AddWork(ThreadedWork[] TW) is not yet implemented");
            //Find which workbuffer has the least entries.

        }

        public static bool TryIssueWorkToThreads(int timeout)
        {
            ThreadedWork[] tw = workToBeAssigned.ToArray();
            workToBeAssigned.Clear();
            WorkBuffer[] _wbs = PackThreadedWorkToWorkBuffers(tw);
            WorkBuffer[] wbs;

            if (FailureBuffer != null && _wbs != null)
            {
                wbs = new WorkBuffer[FailureBuffer.Length + _wbs.Length];
                FailureBuffer.CopyTo(wbs, 0);
                _wbs.CopyTo(wbs, FailureBuffer.Length);
            }
            else if (FailureBuffer == null && _wbs != null)
            {
                wbs = _wbs;
            }
            else
            {
                wbs = FailureBuffer;
            }

            bool wbLock = TryGetLock(timeout);

            if (wbLock)
            {
                int i = 0;

                foreach (WorkBuffer wb in wbs)
                {
                    WorkBuffers[i].Add(wb);
                    i = (i < Threads.Length - 1) ? i + 1 : 0;
                }

                ReleaseLock();
                wbs = null;
            }

            FailureBuffer = wbs;
            return wbLock;
        }

        static WorkBuffer[] PackThreadedWorkToWorkBuffers(ThreadedWork[] tw)
        {
            if (tw != null)
            {
                WorkBuffer[] workbuffers = new WorkBuffer[Mathf.CeilToInt((float)tw.Length / WorkBuffer.MaxBufferSize)];
                int startIndex = 0;
                int maxIndex = 5;
                List<ThreadedWork> twList = new List<ThreadedWork>();
                for (int i = 0; i < workbuffers.Length; i++)
                {
                    startIndex = i * 5;
                    maxIndex = ((i + 1) * 5 <= tw.Length) ? 5 : 5 - (((i + 1) * 5 - tw.Length));

                    for (int y = 0; y < maxIndex; y++)
                    {
                        twList.Add(tw[startIndex + y]);
                    }

                    workbuffers[i] = new WorkBuffer(twList.ToArray());
                    twList.Clear();
                }
                return workbuffers;
            }

            return null;
        }

        public static bool TryGetLock(int timeout)
        {
            return Monitor.TryEnter(lockObject_WorkBuffers, timeout);
        }

        public static void GetLock()
        {
            Monitor.Enter(lockObject_WorkBuffers);
        }

        public static void ReleaseLock()
        {
            Monitor.Exit(lockObject_WorkBuffers);
        }

        public static WorkBuffer[] GetWork(int threadId)
        {
            if (WorkBuffers[threadId].Count > 0)
            {
                WorkBuffer[] wb = WorkBuffers[threadId].ToArray();
                WorkBuffers[threadId].Clear();

                return wb;
            }

            return null;
        }

        public static void EnqueuWork(params ThreadedWork[] tw)
        {
            workToBeAssigned.AddRange(tw);
        }

        public static void RetrieveCompletedWork()
        {
            int i = 0;

            List<CompletedThreadedWork> completedWork = new List<CompletedThreadedWork>();

            foreach (WorkerThread wt in Threads)
            {
                if (wt.TryLockCompletedWorkBuffer(1))
                {
                    completedWork.AddRange(wt.GetCompletedWork());
                    FailedRetrieveAttempts[i] = 0;
                    wt.AllowMoveToPublicCompleteBuffer = true;

                }
                else
                {
                    FailedRetrieveAttempts[i] += 1;
                    if (FailedRetrieveAttempts[i] >= MaxFailedRetrieveAttempts)
                        wt.AllowMoveToPublicCompleteBuffer = false;
                }
                wt.ReleaseCompletedWorkBuffer();
                i++;
            }

            if (completedWork.Count > 0)
                CompletedWorkBuffer.AddRange(completedWork.ToArray());
        }

        public static void ReadCompletedWork(int ReadCounts)
        {
            for (int i = 0; i < ReadCounts; i++)
            {
                ReadCompletedWork(CompletedWorkBuffer[0]);
                CompletedWorkBuffer.RemoveAt(0);
            }
        }

        public static void ReadAllCompletedWork()
        {
            foreach (CompletedThreadedWork ctw in CompletedWorkBuffer)
            {
                ReadCompletedWork(ctw);
            }

            CompletedWorkBuffer.Clear();
        }

        static void ReadCompletedWork(CompletedThreadedWork ctw)
        {
            switch (ctw.Identifier)
            {
                case 2:
                    {
                        ChunkHandler.ReadCompletedThreadedChunkWork(ctw as CompletedThreadedChunkWork);
                        break;
                    }
            }

            ctw.Dispose();
        }

        public static void GetWork()
        {
            bool lockAqquired = false;

            for (int i = 0; i < Threads.Length; i++)
            {
                if (WorkBuffers[i].Count > 0)
                {
                    lockAqquired = Threads[i].TryLockWorkBuffer(2);

                    if (lockAqquired)
                    {
                        Threads[i].AddWork(GetWork(i));
                        Threads[i].ReleaseWorkBuffer();
                        Threads[i].AllowMoveFromPublicWorkBuffer = true;
                        FailedWorkAssignments[i] = 0;
                    }
                    else
                    {
                        FailedWorkAssignments[i] += 1;
                        if (FailedWorkAssignments[i] >= MaxFailedWorkAssignments)
                            Threads[i].AllowMoveFromPublicWorkBuffer = false;
                    }
                }
            }
        }
    }
}
