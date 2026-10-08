using Common.Source.Core.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IGradualTask : IExceptionCaptureOwner
    {
        int RetryCount { get; }

        GradualStates States { get; }

        GradualStates Start();

        GradualStates Retry();

        bool Cancel();

        void Refresh();
    }
}