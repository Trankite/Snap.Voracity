using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Service.Terminal.Interface
{
    public interface IAsyncTerminalCommand : ITerminalCommand
    {
        ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }

    public interface IAsyncTerminalCommand<TContent> : IAsyncTerminalCommand, ITerminalCommand<TContent>
    {
        ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }
}