namespace StarRailDamage.Source.Service.Mission
{
    public class STAThread
    {
        private static readonly Thread? Thread;

        private static readonly Queue<Action> TaskQueue = new();

        private static readonly object ThreadLock = new();

        public static readonly bool IsRunning;

        private static void WorkLoop()
        {
            while (IsRunning)
            {
                lock (ThreadLock)
                {
                    if (TaskQueue.Count == 0)
                    {
                        Monitor.Wait(ThreadLock);
                    }
                }
                if (TaskQueue.Count > 0)
                {
                    TaskQueue.Dequeue().Invoke();
                }
            }
        }

        public static void Invoke(Action task)
        {
            if (IsRunning)
            {
                lock (ThreadLock)
                {
                    TaskQueue.Enqueue(task);
                    Monitor.Pulse(ThreadLock);
                }
            }
            else
            {
                task.Invoke();
            }
        }

        static STAThread()
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                IsRunning = true;
                Thread = new Thread(WorkLoop);
                Thread.SetApartmentState(ApartmentState.STA);
                Thread.Name = nameof(STAThread);
                Thread.IsBackground = true;
                Thread.Start();
            }
        }
    }
}