using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class SemaphoreExtension
    {
        [DebuggerStepThrough]
        public static void ResetCount(this SemaphoreSlim semaphore, int count)
        {
            if (semaphore.CurrentCount < count)
            {
                semaphore.Release(count - semaphore.CurrentCount);
            }
        }

        [DebuggerStepThrough]
        public static async Task WaitReleaseAsync(this SemaphoreSlim semaphore, int count, CancellationToken cancellationToken)
        {
            for (int i = 0; i < count; i++)
            {
                await semaphore.WaitAsync(cancellationToken);
            }
        }
    }
}