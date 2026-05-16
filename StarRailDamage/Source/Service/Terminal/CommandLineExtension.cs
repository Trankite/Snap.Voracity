using StarRailDamage.Source.Extension;
using StarRailDamage.Source.Service.Terminal.Abstraction;

namespace StarRailDamage.Source.Service.Terminal
{
    public static class CommandLineExtension
    {
        public static bool TryGetParameter(this ITerminalCommandLine commandLine, string name, out string result)
        {
            return !string.IsNullOrEmpty(result = commandLine.GetParameter(name));
        }

        public static bool GetBoolParameter(this ITerminalCommandLine commandLine, string name)
        {
            return BoolExtension.Parse(commandLine.GetParameter(name));
        }

        public static int GetIntParameter(this ITerminalCommandLine commandLine, string name)
        {
            return IntExtension.Parse(commandLine.GetParameter(name));
        }

        public static bool TryGetEnumParameter<T>(this ITerminalCommandLine commandLine, string name, out T result) where T : struct, Enum
        {
            return EnumExtension.TryParse(commandLine.GetParameter(name), out result);
        }

        public static double GetDoubleParameter(this ITerminalCommandLine commandLine, string name)
        {
            return DoubleExtension.Parse(commandLine.GetParameter(name));
        }
    }
}