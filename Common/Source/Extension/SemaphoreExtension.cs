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
    }
}