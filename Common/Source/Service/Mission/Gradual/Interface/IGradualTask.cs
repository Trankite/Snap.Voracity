using Common.Source.Service.Mission.Gradual.Metadata;
using Common.Source.Service.Mission.Interface;

namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IGradualTask : IExceptionCapturer
    {
        int RetryCount { get; }

        GradualStatus Status { get; }

        void Start();

        void Retry();

        void Cancel();

        void Refresh();
    }
}