using Common.Source.Extension;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service.Dispose
{
    public sealed class StopwatchOwner : IDisposable
    {
        public Stopwatch Stopwatch { get; }

        public StopwatchOwner(Stopwatch stopwatch)
        {
            Stopwatch = stopwatch;
        }

        [return: NotNullIfNotNull(nameof(stopwatch))]
        public static StopwatchOwner? Create(Stopwatch? stopwatch, bool leaveStop = default)
        {
            if (stopwatch.IsNotNull())
            {
                if (!leaveStop)
                {
                    stopwatch.Start();
                }
                return new StopwatchOwner(stopwatch);
            }
            return default;
        }

        public void Dispose()
        {
            Stopwatch.Stop();
        }
    }
}