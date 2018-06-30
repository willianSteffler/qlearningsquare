using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Shared
{
    public delegate void SafeThAction (SafeTh sender, params object[] args);
    public delegate void SafeThExc(SafeTh sender, Exception e);
    

    public class SafeTh : IDisposable
    {

        static List<SafeTh> threads = new List<SafeTh>();
        List<SafeTh> instanceThreads = new List<SafeTh>();
        Thread pthread = null;
        volatile bool running;
        ThreadPriority priority = ThreadPriority.Normal;

        /// <summary>
        /// Delegate to be executed when an unhandled exception occurs on the safeTh's action
        /// </summary>
        public SafeThExc OnException = delegate (SafeTh sender, Exception e) {
            throw e;
        };

        public bool Loop = false;
        public int LoopIntervalms = 0;

        public ThreadPriority Priority
        {
            get => priority;
            set {
                priority = value;
                if (pthread != null)
                {
                    pthread.Priority = priority;
                }
            }
        }

        public bool Running { get => pthread!=null && pthread.IsAlive && running; }

        /// <summary>
        /// Start the safeTh action
        /// </summary>
        /// <param name="action">Action to be performed by the safeTh</param>
        /// <param name="actionArgs">Arguments to be taken by the safeTh</param>
        public void Start(SafeThAction action,params object[] actionArgs)
        {
            threads.Add(this);

            pthread = new Thread(delegate ()
            {
                try
                {
                    running = Loop;

                    do
                    {
                        pthread.Priority = priority;
                        action(this, actionArgs);

                        if (Loop)
                            Thread.Sleep(LoopIntervalms);
                    }
                    while (pthread.IsAlive && running);
                }
                catch (Exception e)
                {
                    OnException(this,e);
                }
            });
            pthread.Start();
        }

        /// <summary>
        /// Starts a new safeTh as a child from the current safeTh instance
        /// </summary>
        /// <param name="th">object that references the safeTh</param>
        /// <param name="action">action to be performed for the safeTh</param>
        /// <param name="actionArgs">arguments to be taken by the safeTh action</param>
        public void StartNew(SafeTh th,SafeThAction action,params object[] actionArgs)
        {
            if(th != null)
            {
                this.instanceThreads.Add(th);
                if(th.pthread == null)
                {
                    th.Start(action, actionArgs);
                }
            }
        }

        /// <summary>
        /// Stops the safeTh execution
        /// </summary>
        public void Stop()
        {
            running = false;
            pthread.Join();
        }

        /// <summary>
        /// Suspend the thread for the specified number of milisseconds
        /// </summary>
        /// <param name="ms"> milisseconds to wait</param>
        public void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }

        /// <summary>
        /// Stop all the safeThs created
        /// </summary>
        public static void StopAllThreads()
        {
            Parallel.ForEach(threads, delegate (SafeTh currInstance)
            {
                currInstance.Stop();
            });
        }

        /// <summary>
        /// Stop all the safeThs started by this safeTh
        /// </summary>
        public void StopAssociatedThreads()
        {
            instanceThreads.ForEach(th => th.Stop());
        }

        /// <summary>
        /// Stops safeThs and clears the object
        /// </summary>
        public void Dispose()
        {
            this.Stop();
            StopAssociatedThreads();
            instanceThreads.Clear();
            instanceThreads = null;
        }
    }
}
