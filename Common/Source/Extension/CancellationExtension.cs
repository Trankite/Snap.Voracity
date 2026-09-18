using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class CancellationExtension
    {
        [DebuggerStepThrough]
        public static CancellationTokenSource CreateLinkedTokenSource(this CancellationToken cancellationToken)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        }

        [DebuggerStepThrough]
        public static bool IsUnCanceledOrThrow(this CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return !cancellationToken.IsCancellationRequested;
        }

        [DebuggerStepThrough]
        public static bool TryCancel(this CancellationTokenSource cancellationSource)
        {
            try { cancellationSource.Cancel(); return true; } catch { return false; }
        }
    }
}