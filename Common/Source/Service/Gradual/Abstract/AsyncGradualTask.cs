using Common.Source.Extension;
using Common.Source.Service.Gradual.Interface;
using Common.Source.Service.Gradual.Metadata;

namespace Common.Source.Service.Gradual.Abstract
{
    public abstract class AsyncGradualTask : GradualTask, IAsyncGradualTask
    {
        public async ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            if (!Status.IsCreated())
            {
                return;
            }
            CapturedException = default;
            try
            {
                Status = await StartAsyncOverride(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Status = FailedByException(exception);
            }
        }

        protected override GradualStatus StartOverride()
        {
            return StartAsyncOverride().GetAwaitResult();
        }

        protected virtual async ValueTask<GradualStatus> StartAsyncOverride(CancellationToken cancellationToken = default)
        {
            return await RetryAsyncOverride(cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask RetryAsync(CancellationToken cancellationToken = default)
        {
            if (!Status.IsRunning())
            {
                return;
            }
            CapturedException = default;
            try
            {
                Status = await RetryAsyncOverride(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Status = FailedByException(exception);
            }
            finally
            {
                RetryCount++;
            }
        }

        protected override GradualStatus RetryOverride()
        {
            return RetryAsyncOverride().GetAwaitResult();
        }

        protected abstract ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default);
    }
}