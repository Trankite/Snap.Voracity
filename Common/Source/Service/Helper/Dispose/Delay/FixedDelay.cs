using Common.Source.Service.Helper.Dispose.Delay.Interface;
using System.Diagnostics;

namespace Common.Source.Service.Helper.Dispose.Delay
{
    public sealed class FixedDelay : IDelayer
    {
        public TimeSpan Delay { get; }

        private readonly Stopwatch Watch = new();

        public FixedDelay(TimeSpan delay)
        {
            Delay = delay;
        }

        public void Start()
        {
            Watch.Restart();
        }

        public async ValueTask WaitAsync(CancellationToken cancellationToken = default)
        {
            TimeSpan waitTime = Delay - Watch.Elapsed;
            if (waitTime > TimeSpan.Zero)
            {
                try { await Task.Delay(waitTime, cancellationToken).ConfigureAwait(false); } catch { }
            }
        }
    }
}