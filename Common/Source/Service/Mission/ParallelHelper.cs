namespace Common.Source.Service.Mission
{
    public static class ParallelHelper
    {
        public static ParallelOptions CreateOptions(int maxDegreeOfParallelism, TaskScheduler? taskScheduler = default, CancellationToken cancellationToken = default)
        {
            return new ParallelOptions()
            {
                TaskScheduler = taskScheduler,
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cancellationToken
            };
        }
    }
}