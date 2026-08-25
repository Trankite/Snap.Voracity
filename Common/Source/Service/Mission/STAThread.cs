namespace Common.Source.Service.Mission
{
    public class STAThread
    {
        private static readonly Thread? Thread;

        private static readonly Queue<Task> TaskQueue = [];

        private static readonly object ThreadLock = new();

        public static readonly bool IsRunning;

        private static void WorkLoop()
        {
            while (IsRunning)
            {
                Task? Current = null;
                lock (ThreadLock)
                {
                    while (!TaskQueue.TryDequeue(out Current))
                    {
                        Monitor.Wait(ThreadLock);
                    }
                }
                Current.RunSynchronously();
            }
        }

        public static Task Run(Action action)
        {
            if (IsRunning)
            {
                Task Task = new(action);
                lock (ThreadLock)
                {
                    TaskQueue.Enqueue(Task);
                    Monitor.Pulse(ThreadLock);
                }
                return Task;
            }
            else
            {
                return Task.Run(action);
            }
        }

        public static async void Start(Action action) => await Run(action);

        static STAThread()
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                IsRunning = true;
                Thread = new Thread(WorkLoop)
                {
                    IsBackground = true,
                    Name = nameof(STAThread)
                };
                if (OperatingSystem.IsWindows())
                {
                    Thread.SetApartmentState(ApartmentState.STA);
                }
                Thread.Start();
            }
        }
    }
}