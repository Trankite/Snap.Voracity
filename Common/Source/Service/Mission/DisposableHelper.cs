using System.Diagnostics;

namespace Common.Source.Service.Mission
{
    public static class DisposableHelper
    {
        [DebuggerStepThrough]
        public static void DisposeAndSetNull<T>(ref T? disposable) where T : IDisposable
        {
            disposable?.Dispose();
            disposable = default;
        }
    }
}