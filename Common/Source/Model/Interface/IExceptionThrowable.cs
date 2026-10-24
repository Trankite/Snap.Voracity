namespace Common.Source.Model.Interface
{
    public interface IExceptionThrowable
    {
        bool IsReady { get; }

        void ThrowIfFailed();
    }
}