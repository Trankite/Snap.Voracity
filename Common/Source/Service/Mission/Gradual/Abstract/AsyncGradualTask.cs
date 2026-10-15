using Common.Source.Extension;
using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Abstract
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
            catch (Exception Exception)
            {
                Status = FailedByException(Exception.DispatchCapture());
            }
        }

        protected override GradualStatus StartOverride()
        {
            return StartAsyncOverride().AsTask().GetAwaiter().GetResult();
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
            catch (Exception Exception)
            {
                Status = FailedByException(Exception.DispatchCapture());
            }
            finally
            {
                RetryCount++;
            }
        }

        protected override GradualStatus RetryOverride()
        {
            return RetryAsyncOverride().AsTask().GetAwaiter().GetResult();
        }

        protected abstract ValueTask<GradualStatus> RetryAsyncOverride(CancellationToken cancellationToken = default);
    }
}