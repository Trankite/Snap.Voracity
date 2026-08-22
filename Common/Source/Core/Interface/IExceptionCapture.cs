using System.Runtime.ExceptionServices;

namespace Common.Source.Core.Interface
{
    public interface IExceptionCapture
    {
        ExceptionDispatchInfo? Exception { get; }
    }
}