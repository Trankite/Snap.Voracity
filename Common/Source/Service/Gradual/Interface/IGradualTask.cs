using Common.Source.Model.Interface;
using Common.Source.Service.Gradual.Metadata;

namespace Common.Source.Service.Gradual.Interface
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