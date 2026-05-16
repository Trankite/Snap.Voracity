using StarRailDamage.Source.Service.Terminal.Abstraction;
using System.Diagnostics.CodeAnalysis;

namespace StarRailDamage.Source.Service.Terminal
{
    public class TerminalCommand : IAsyncTerminalCommand
    {
        [MemberNotNullWhen(true, nameof(AsyncCommand))]
        public bool IsAsync { get; }

        public ITerminalCommand Command { get; }

        public IAsyncTerminalCommand? AsyncCommand { get; }

        public string Name => Command.Name;

        public string Help => Command.Help;

        public string[] Parameters => Command.Parameters;

        private TerminalCommand(bool isAsync, ITerminalCommand command)
        {
            IsAsync = isAsync;
            Command = command;
        }

        public TerminalCommand(IAsyncTerminalCommand asyncCommand) : this(true, asyncCommand)
        {
            AsyncCommand = asyncCommand;
        }

        public static TerminalCommand Create(ITerminalCommand command)
        {
            return command is IAsyncTerminalCommand AsyncCommand ? new TerminalCommand(AsyncCommand) : new TerminalCommand(false, command);
        }

        public ITerminalResponse Invoke(ITerminalCommandLine commandLine) => Command.Invoke(commandLine);

        public async ValueTask<ITerminalResponse> AsyncInvoke(ITerminalCommandLine commandLine)
        {
            return IsAsync ? await AsyncCommand.AsyncInvoke(commandLine) : Invoke(commandLine);
        }
    }
}