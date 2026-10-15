using Common.Source.Core.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IGradualTask : IExceptionCaptureOwner
    {
        int RetryCount { get; }

        GradualStatus Status { get; }

        void Start();

        void Retry();

        void Cancel();

        void Refresh();
    }
}