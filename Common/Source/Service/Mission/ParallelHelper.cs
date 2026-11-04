using System.Diagnostics;

namespace Common.Source.Service.Mission
{
    public static class ParallelHelper
    {
        [DebuggerStepThrough]
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