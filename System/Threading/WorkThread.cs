using System;
using System.Threading;

namespace Threading
{
    public class WorkThread
    {
        readonly ThreadManager _ThreadManager;

        internal WorkThread(ThreadManager threadManager)
        {
            _ThreadManager = threadManager;
        }

        public static void AddWorker(ThreadManager threadManager)
        {
            Interlocked.Increment(ref threadManager.activeThreads);
            var worker = new WorkThread(threadManager);
            var thread = new Thread(worker.Loop);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        void Loop()
        {
            try
            {
                _ThreadManager.ThreadStartedEvent();

                while (true)
                {
                    WorkItem item = null;
                    try
                    {
                        item = (WorkItem) _ThreadManager.workItems.Pop();
                    }

                    catch
                    {
                        Thread.Sleep(_ThreadManager.threadTimeout);
                        item = (WorkItem) _ThreadManager.workItems.Pop();
                        if (item == null) break;
                    }
                    try
                    {
                        _ThreadManager.JobStartedEvent(item.Parameter);
                        /*
                         Write your functions you want to process  
                         * Here we cast the item.parameter into the 
                         * ProcessingSteps and get the type to run
                         * According to the type related functions
                         * will be called.
                         * E.g. if it is CreateProcess then 
                         * CreatePrcess of ProcessEngine will be called.
                         * Same way functions called for each parameter.
                         * 
                        */
                        //item.callback(item.parameter);
                        // Following is my function and it is called by passing parameter.
                        //ProcessingManager.Process((Processingstep)item.parameter);
                        _ThreadManager.JobFinishedEvent(item.Parameter);
                    }

                    catch (Exception exp)
                    {
                        try
                        {
                            //EngineLog.UpdateLogLine(exp.ToString());
                            _ThreadManager.OnErrorEvent(exp);
                        }

                        catch (Exception critical)
                        {
                            // Check for critical exception -- pseudo code
                            //if (critical is CRITICAL)
                            throw critical;
                        }
                    }

                    if (_ThreadManager.activeThreads > _ThreadManager.MaxThreads) return;
                }
            }
            catch {}
            finally
            {
                Interlocked.Decrement(ref _ThreadManager.activeThreads);
            }

            _ThreadManager.ThreadFinishedEvent();
            if (_ThreadManager.activeThreads == 0)
            {
                _ThreadManager.IdleEvent();
            }
        }
    }
}