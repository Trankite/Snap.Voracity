using Common.Source.Factory.Streams.Text.Interface;
using Common.Source.Service.Terminal.Interface;

namespace Common.Source.Service.Terminal.Abstract
{
    public abstract class AsyncTerminalCommand : TerminalCommand, IAsyncTerminalCommand
    {
        public override ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return AsyncInvoke(commandLine, textStream, cancellationToken).AsTask().GetAwaiter().GetResult();
        }

        public abstract ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }

    public abstract class AsyncTerminalCommand<TContent> : AsyncTerminalCommand, IAsyncTerminalCommand<TContent>
    {
        public override async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return await AsyncInvokeOverride(commandLine, textStream, cancellationToken).ConfigureAwait(false);
        }

        public ITerminalResponse<TContent> InvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default)
        {
            return AsyncInvokeOverride(commandLine, textStream, cancellationToken).AsTask().GetAwaiter().GetResult();
        }

        public abstract ValueTask<ITerminalResponse<TContent>> AsyncInvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }
}