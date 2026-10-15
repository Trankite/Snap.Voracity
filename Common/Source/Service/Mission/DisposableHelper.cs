namespace Common.Source.Service.Mission
{
    public static class DisposableHelper
    {
        public static void DisposeAndSetNull<T>(ref T? disposable) where T : IDisposable
        {
            disposable?.Dispose();
            disposable = default;
        }
    }
}