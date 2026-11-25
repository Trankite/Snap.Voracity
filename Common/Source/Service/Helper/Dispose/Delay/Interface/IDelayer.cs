namespace Common.Source.Service.Helper.Dispose.Delay.Interface
{
    public interface IDelayer
    {
        void Start();

        ValueTask WaitAsync(CancellationToken cancellationToken = default);
    }
}