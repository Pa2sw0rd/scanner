using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadManager
{
    public class ThreadPoolManager
    {
        private int MaxThreadNum;
        private int MinThreadNum;
        private int GrowStepNum;
        //线程数量
        public int ThreadNum { get; set; }
        //默认线程数量
        public int DefaultThreadNum { get; set; }

        private Queue<Task> TaskQueue;
        private Queue<WorkThread> WorkThreadList;

        public ThreadPoolManager(int i)
        {
            TaskQueue = new Queue<Task>();
            WorkThreadList = new Queue<WorkThread>();
            DefaultThreadNum = 10;
            if (i > 0)
                DefaultThreadNum = i;
            CreateThreadPool(i);
        }
        public ThreadPoolManager() : this(10)
        {
        }
        public bool IsAllTaskFinish()
        {
            return TaskQueue.Count == 0;
        }
        public void CreateThreadPool(int i)
        {
            if (WorkThreadList == null)
                WorkThreadList = new Queue<WorkThread>();
            lock (WorkThreadList)
            {
                for (int j = 0; j < i; j++)
                {
                    ThreadNum++;
                    WorkThread workthread = new WorkThread(ref TaskQueue, ThreadNum);
                    WorkThreadList.Enqueue(workthread);
                }
            }
        }
        public void AddTask(Task task)
        {

            if (task == null)
                return;
            lock (TaskQueue)
            {
                TaskQueue.Enqueue(task);
            }
            //Monitor.Enter(TaskQueue);
            //TaskQueue.Enqueue(task);
            //Monitor.Exit(TaskQueue);
        }
        public void CloseThread()
        {
            //Object obj = null;
            while (WorkThreadList.Count != 0)
            {
                try
                {
                    WorkThread workthread = WorkThreadList.Dequeue();
                    workthread.CloseThread();
                    continue;
                }
                catch (Exception)
                {
                }
                break;
            }
        }
    }





    public class WorkThread
    {
        public int ThreadNum { get; set; }
        private bool flag;
        private Queue<Task> TaskQueue;
        private Task task;
        public WorkThread(ref Queue<Task> queue, int i)
        {
            this.TaskQueue = queue;
            ThreadNum = i;
            flag = true;
            new Thread(run).Start();
        }
        public void run()
        {
            while (flag && TaskQueue != null)
            {
                //获取任务
                lock (TaskQueue)
                {
                    try
                    {
                        task = TaskQueue.Dequeue();
                    }
                    catch (Exception)
                    {
                        task = null;
                    }
                    if (task == null)
                        continue;
                }
                try
                {

                    //task.SetEnd(false);
                    //task.StartTask();
                    task.Start();
                }
                catch (Exception)
                {
                }
                try
                {
                    /*
                    if (!task.IsEnd())
                    {
                        //task.SetEnd(false);
                        //task.EndTask();
                    }*/
                }
                catch (Exception)
                {
                }

            }//end of while
        }
        public void CloseThread()
        {
            flag = false;
            try
            {
                if (task != null)
                {
                    //task.EndTask();
                    task.Dispose();
                }
                
                    
            }
            catch (Exception)
            {
            }
        }
    }


    /*
    public interface Task
    {
        /// <summary>
        /// set flag of task.
        /// </summary>
        void SetEnd(bool flag);
        /// <summary>
        /// start task.
        /// </summary>
        void StartTask();
        /// <summary>
        /// end task.
        /// </summary>
        void EndTask();
        /// <summary>
        /// get status of task.
        /// </summary>
        /// <returns></returns>
        bool IsEnd();
    }



    public class TestTask : Task
    {
        private bool is_end;
        public void SetEnd(bool flag)
        {
            is_end = flag;
        }
        public void StartTask()
        {
            Run();
        }
        public void EndTask()
        {
            is_end = true;
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + "结束！");
        }
        public bool IsEnd()
        {
            return is_end;
        }
        public void Run()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" + i);
            }
        }

    }
    */


}
