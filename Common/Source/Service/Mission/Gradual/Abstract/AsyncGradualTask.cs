using Common.Source.Extension;
using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Abstract
{
    public abstract class AsyncGradualTask : GradualTask, IAsyncGradualTask
    {
        public async ValueTask<GradualStates> StartAsync(CancellationToken cancellationToken = default)
        {
            CapturedException = default;
            try
            {
                return States == GradualStates.Created ? States = await StartAsyncOverride(cancellationToken).ConfigureAwait(false) : States;
            }
            catch (Exception Exception)
            {
                return States = GradualStates.Suspend.Configure(this.DispatchCapture(Exception));
            }
        }

        protected override GradualStates StartOverride()
        {
            return StartAsync().AsTask().GetAwaiter().GetResult();
        }

        protected virtual async ValueTask<GradualStates> StartAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await RetryAsyncOverride(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<GradualStates> RetryAsync(CancellationToken cancellationToken = default)
        {
            CapturedException = default;
            try
            {
                return States == GradualStates.Suspend ? States = (await RetryAsyncOverride(cancellationToken).ConfigureAwait(false)).Configure(RetryCount++) : States;
            }
            catch (Exception Exception)
            {
                return States = GradualStates.Suspend.Configure(this.DispatchCapture(Exception));
            }
        }

        protected override GradualStates RetryOverride()
        {
            return RetryAsync().AsTask().GetAwaiter().GetResult();
        }

        protected abstract ValueTask<GradualStates> RetryAsyncOverride(CancellationToken cancellationToken = default);
    }

    public abstract class AsyncGradualTask<T> : AsyncGradualTask, IAsyncGradualTask<T>
    {
        public T? GradualResult { get; protected set; }
    }
}