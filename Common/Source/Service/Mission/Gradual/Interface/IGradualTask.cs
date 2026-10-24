using Common.Source.Model.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IGradualTask : IExceptionable
    {
        int RetryCount { get; }

        GradualStatus Status { get; }

        void Start();

        void Retry();

        void Cancel();

        void Refresh();
    }
}