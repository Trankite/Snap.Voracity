using System.Runtime.ExceptionServices;

namespace Common.Source.Model.Interface
{
    public interface IExceptionable
    {
        ExceptionDispatchInfo? CapturedException { get; }
    }
}