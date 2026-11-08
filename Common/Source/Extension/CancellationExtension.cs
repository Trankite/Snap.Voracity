using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class CancellationExtension
    {
        [DebuggerStepThrough]
        public static CancellationTokenSource CreateSource(this CancellationToken cancellationToken)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        }

        [DebuggerStepThrough]
        public static async Task CancelAfterTaskAsync(this CancellationTokenSource cancellationTokenSource, Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            finally
            {
                cancellationTokenSource.Cancel();
            }
        }
    }
}