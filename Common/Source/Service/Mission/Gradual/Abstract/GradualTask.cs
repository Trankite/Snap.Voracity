using Common.Source.Extension;
using Common.Source.Service.Mission.Gradual.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;
using System.Runtime.ExceptionServices;

namespace Common.Source.Service.Mission.Gradual.Abstract
{
    public abstract class GradualTask : IGradualTask
    {
        private GradualStates _States;

        public int RetryCount { get; protected set; }

        public ExceptionDispatchInfo? CapturedException { get; set; }

        public GradualStates States
        {
            get => _States;
            protected set => _States = value;
        }

        protected GradualTask() { }

        public GradualStates Start()
        {
            CapturedException = default;
            try
            {
                return States == GradualStates.Created ? States = StartOverride() : States;
            }
            catch (Exception Exception)
            {
                return States = GradualStates.Suspend.Configure(this.DispatchCapture(Exception));
            }
        }

        protected virtual GradualStates StartOverride() => RetryOverride();

        public GradualStates Retry()
        {
            CapturedException = default;
            try
            {
                return States == GradualStates.Suspend ? States = RetryOverride().Configure(RetryCount++) : States;
            }
            catch (Exception Exception)
            {
                return States = GradualStates.Suspend.Configure(this.DispatchCapture(Exception));
            }
        }

        protected abstract GradualStates RetryOverride();

        public virtual void ThrowIfFailed()
        {
            this.ThrowIfExceptionCaptured();
        }

        protected GradualStates FailedByException(ExceptionDispatchInfo? exceptionDispatchInfo = default)
        {
            return GradualStates.Suspend.Configure(CapturedException = exceptionDispatchInfo);
        }

        public bool Cancel()
        {
            return GradualStates.Faulted.IsExchanged(ref _States);
        }

        public void Refresh()
        {
            RefreshOverride();
            States = GradualStates.Created;
            RetryCount = 0;
        }

        protected abstract void RefreshOverride();
    }
}