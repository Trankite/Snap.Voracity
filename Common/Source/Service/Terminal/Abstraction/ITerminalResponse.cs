using Common.Source.Core.Interface;

namespace Common.Source.Service.Terminal.Abstraction
{
    public interface ITerminalResponse
    {
        bool Success { get; }

        string Message { get; }
    }

    public interface ITerminalResponse<TContent> : ITerminalResponse, IResponseAnalyzedBody<TContent>
    {
        TContent? Content { get; }
    }
}