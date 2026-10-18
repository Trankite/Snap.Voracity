using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;
using System.Diagnostics;

namespace Common.Source.Service.Mission.Gradual.Abstract
{
    public static class GradualTaskExtension
    {
        [DebuggerStepThrough]
        public static T StartOrRetry<T>(this T gradualTask) where T : IGradualTask
        {
            if (gradualTask.Status.IsCreated())
            {
                gradualTask.Start();
            }
            else if (gradualTask.Status.IsRunning())
            {
                gradualTask.Retry();
            }
            return gradualTask;
        }

        [DebuggerStepThrough]
        public static async ValueTask<T> StartOrRetryAsync<T>(this T gradualTask, CancellationToken cancellationToken = default) where T : IAsyncGradualTask
        {
            if (gradualTask.Status.IsCreated())
            {
                await gradualTask.StartAsync(cancellationToken).ConfigureAwait(false);
            }
            else if (gradualTask.Status.IsRunning())
            {
                await gradualTask.RetryAsync(cancellationToken).ConfigureAwait(false);
            }
            return gradualTask;
        }
    }
}