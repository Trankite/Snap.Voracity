namespace Common.Source.Service.Mission.Gradual.Interface
{
    public interface IAsyncGradualTask : IGradualTask
    {
        ValueTask StartAsync(CancellationToken cancellationToken = default);

        ValueTask RetryAsync(CancellationToken cancellationToken = default);
    }
}