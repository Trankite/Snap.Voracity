using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;
using System.Diagnostics;

namespace Common.Source.Service.Mission.Gradual.Abstract
{
    public static class GradualTaskExtension
    {
        [DebuggerStepThrough]
        public static GradualStates StartOrRetry(this IGradualTask gradualTask)
        {
            return gradualTask.States == GradualStates.Created ? gradualTask.Start() : gradualTask.Retry();
        }

        [DebuggerStepThrough]
        public static async ValueTask<GradualStates> StartOrRetryAsync(this IAsyncGradualTask gradualTask, CancellationToken cancellationToken = default)
        {
            return gradualTask.States == GradualStates.Created ? await gradualTask.StartAsync(cancellationToken).ConfigureAwait(false) : await gradualTask.RetryAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}