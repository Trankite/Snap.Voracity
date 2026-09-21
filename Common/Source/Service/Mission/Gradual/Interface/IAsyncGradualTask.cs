using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IAsyncGradualTask : IGradualTask
    {
        ValueTask<GradualStates> StartAsync(CancellationToken cancellationToken = default);

        ValueTask<GradualStates> RetryAsync(CancellationToken cancellationToken = default);
    }
}