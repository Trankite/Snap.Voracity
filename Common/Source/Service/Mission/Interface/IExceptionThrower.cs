namespace Common.Source.Service.Mission.Interface
{
    public interface IExceptionThrower
    {
        bool IsReady { get; }

        void ThrowIfFailed();
    }
}