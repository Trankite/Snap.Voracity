using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Service.Terminal.Interface
{
    public interface ITerminalCommand
    {
        string Name { get; }

        string FullName { get; }

        string Help { get; }

        string[] RequiredParameters { get; }

        string[] OptionalParameters { get; }

        ITerminalResponse Invoke(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }

    public interface ITerminalCommand<TContent> : ITerminalCommand
    {
        ITerminalResponse<TContent> InvokeOverride(ITerminalCommandLine commandLine, ITextStream? textStream = default, CancellationToken cancellationToken = default);
    }
}