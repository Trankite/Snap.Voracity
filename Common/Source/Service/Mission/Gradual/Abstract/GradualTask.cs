using Common.Source.Extension;
using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;
using System.Runtime.ExceptionServices;

namespace Common.Source.Service.Mission.Gradual.Abstract
{
    public abstract class GradualTask : IGradualTask
    {
        public int RetryCount { get; protected set; }

        public ExceptionDispatchInfo? CapturedException { get; set; }

        public GradualStatus Status { get; protected set; }

        protected GradualTask() { }

        public void Start()
        {
            if (!Status.IsCreated())
            {
                return;
            }
            CapturedException = default;
            try
            {
                Status = StartOverride();
            }
            catch (Exception Exception)
            {
                Status = FailedByException(Exception.DispatchCapture());
            }
        }

        protected virtual GradualStatus StartOverride() => RetryOverride();

        public void Retry()
        {
            if (!Status.IsRunning())
            {
                return;
            }
            CapturedException = default;
            try
            {
                Status = RetryOverride();
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

        protected abstract GradualStatus RetryOverride();

        public virtual void ThrowIfFailed()
        {
            this.ThrowIfExceptionCaptured();
        }

        protected GradualStatus FailedByException(ExceptionDispatchInfo? exceptionDispatchInfo = default)
        {
            if (exceptionDispatchInfo.IsNotNull())
            {
                CapturedException = exceptionDispatchInfo;
            }
            return GradualStatus.Running;
        }

        public void Cancel()
        {
            CancelOverride();
            Status = GradualStatus.Canceled;
        }

        protected virtual void CancelOverride() { }

        public void Refresh()
        {
            RefreshOverride();
            Status = GradualStatus.Created;
            RetryCount = 0;
        }

        protected abstract void RefreshOverride();
    }
}