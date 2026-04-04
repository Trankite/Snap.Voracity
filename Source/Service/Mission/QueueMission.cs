namespace StarRailDamage.Source.Service.Mission
{
    public class QueueMission : IQueueMission
    {
        private readonly Func<bool> Mission;

        public int Attempt { get; set; }

        public int MaxAttempt { get; private set; }

        public QueueMission(Func<bool> mission, int maxAttempt = 1)
        {
            Mission = mission;
            MaxAttempt = maxAttempt;
        }

        public bool Invoke()
        {
            return Mission.Invoke();
        }
    }
}