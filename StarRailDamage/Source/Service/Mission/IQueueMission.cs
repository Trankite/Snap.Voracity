namespace StarRailDamage.Source.Service.Mission
{
    public interface IQueueMission
    {
        bool Invoke();

        int Attempt { get; set; }

        int MaxAttempt { get; }
    }
}