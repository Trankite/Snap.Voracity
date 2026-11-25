namespace Common.Source.Service.Helper.Dispose.Delay.Interface
{
    public interface IDelayable
    {
        IDelayer Delayer { get; set; }
    }
}