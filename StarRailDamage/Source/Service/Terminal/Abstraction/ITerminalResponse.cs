using StarRailDamage.Source.Core.Abstraction;

namespace StarRailDamage.Source.Service.Terminal.Abstraction
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