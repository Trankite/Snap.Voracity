using Common.Source.Service.Helper.Dispose.Delay.Interface;

namespace Common.Source.Service.Helper.Dispose.Delay.Abstract
{
    public static class Delayable
    {
        private static readonly TimeSpan DefaultDelay;

        public static IDelayer Zero { get; } = new NullDelayer();

        public static IDelayer Default => new FixedDelay(DefaultDelay);

        private class NullDelayer : IDelayer
        {
            public void Start() { }

            public async ValueTask WaitAsync(CancellationToken cancellationToken = default) { }
        }

        static Delayable()
        {
            DefaultDelay = TimeSpan.FromMilliseconds(500);
        }
    }
}