using Common.Source.Core.Interface;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IGradualTask : IExceptionCapture
    {
        int RetryCount { get; }

        GradualStates States { get; }

        GradualStates Start();

        GradualStates Retry();

        bool Cancel();
    }

    public interface IGradualTask<T> : IGradualTask
    {
        T? GradualResult { get; }
    }
}