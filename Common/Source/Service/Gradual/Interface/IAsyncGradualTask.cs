namespace Common.Source.Service.Gradual.Interface
{
    public interface IAsyncGradualTask : IGradualTask
    {
        ValueTask StartAsync(CancellationToken cancellationToken = default);

        ValueTask RetryAsync(CancellationToken cancellationToken = default);
    }
}