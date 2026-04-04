using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal
{
    public class TerminalCommand : IAsyncTerminalCommand
    {
        public bool IsAsync { get; }

        public ITerminalCommand Command { get; }

        public IAsyncTerminalCommand AsyncCommand => (IAsyncTerminalCommand)Command;

        public string Name => Command.Name;

        public string Help => Command.Help;

        private TerminalCommand(bool isAsync, ITerminalCommand command)
        {
            IsAsync = isAsync;
            Command = command;
        }

        public static TerminalCommand Create(ITerminalCommand command)
        {
            return new TerminalCommand(command is IAsyncTerminalCommand, command);
        }

        public static TerminalCommand Create(IAsyncTerminalCommand command)
        {
            return new TerminalCommand(true, command);
        }

        public ITerminalResponse Invoke(params string[] parameter)
        {
            return Command.Invoke(parameter);
        }

        public async ValueTask<ITerminalResponse> AsyncInvoke(params string[] parameter)
        {
            return IsAsync ? await AsyncCommand.AsyncInvoke(parameter) : Invoke(parameter);
        }
    }
}