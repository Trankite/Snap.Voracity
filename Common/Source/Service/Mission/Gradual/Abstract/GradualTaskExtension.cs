using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;
using System.Diagnostics;

namespace Common.Source.Service.Mission.Gradual.Abstract
{
    public static class GradualTaskExtension
    {
        [DebuggerStepThrough]
        public static void StartOrRetry(this IGradualTask gradualTask)
        {
            if (gradualTask.Status.IsCreated())
            {
                gradualTask.Start();
            }
            else if (gradualTask.Status.IsRunning())
            {
                gradualTask.Retry();
            }
        }

        [DebuggerStepThrough]
        public static async ValueTask StartOrRetryAsync(this IAsyncGradualTask gradualTask, CancellationToken cancellationToken = default)
        {
            if (gradualTask.Status.IsCreated())
            {
                await gradualTask.StartAsync(cancellationToken).ConfigureAwait(false);
            }
            else if (gradualTask.Status.IsRunning())
            {
                await gradualTask.RetryAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}