namespace StarRailDamage.Source.Service.Mission
{
    public class MissionQueue
    {
        public Queue<IQueueMission> Queue { get; private set; } = [];

        public MissionQueue() { }

        public MissionQueue(Queue<IQueueMission> queue)
        {
            Queue = queue;
        }

        public bool Invoke()
        {
            Queue<IQueueMission> Failed = [];
            while (Queue.TryDequeue(out IQueueMission? Mission))
            {
                if (Mission.Attempt < Mission.MaxAttempt)
                {
                    if (!Mission.Invoke())
                    {
                        Mission.Attempt++;
                        Queue.Enqueue(Mission);
                    }
                }
                else
                {
                    Failed.Enqueue(Mission);
                }
            }
            return (Queue = Failed).Count == 0;
        }
    }
}