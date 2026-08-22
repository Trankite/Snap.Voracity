using Common.Source.Extension;
using Common.Source.Service.Terminal.Abstraction;

namespace Common.Source.Service.Terminal
{
    public static class CommandLineExtension
    {
        public static string GetParameter(this ITerminalCommandLine commandLine, string name)
        {
            return commandLine.TryGetParameter(name, out string? Result) ? Result : string.Empty;
        }

        public static bool GetBoolParameter(this ITerminalCommandLine commandLine, string name)
        {
            return BoolExtension.Parse(commandLine.GetParameter(name));
        }

        public static int GetIntParameter(this ITerminalCommandLine commandLine, string name)
        {
            return IntegerExtension.Parse(commandLine.GetParameter(name));
        }

        public static double GetDoubleParameter(this ITerminalCommandLine commandLine, string name)
        {
            return DoubleExtension.Parse(commandLine.GetParameter(name));
        }
    }
}