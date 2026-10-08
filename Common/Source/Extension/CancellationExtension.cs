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
        public static async Task CancelAfterAsync(this CancellationTokenSource cancellationTokenSource, Task waitTask)
        {
            try
            {
                await waitTask.ConfigureAwait(false);
            }
            finally
            {
                cancellationTokenSource.Cancel();
            }
        }
    }
}