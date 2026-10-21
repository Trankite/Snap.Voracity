using System.Diagnostics;

namespace Common.Source.Service.Mission
{
    public static class DisposableHelper
    {
        [DebuggerStepThrough]
        public static T? Dispose<T>(T? disposable) where T : IDisposable
        {
            disposable?.Dispose();
            return default;
        }
    }
}