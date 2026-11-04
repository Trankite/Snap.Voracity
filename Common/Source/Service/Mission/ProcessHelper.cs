using Common.Source.Extension;
using System.Diagnostics;

namespace Common.Source.Service.Mission
{
    public static class ProcessHelper
    {
        [StackTraceHidden]
        [DebuggerStepThrough]
        public static Process Start(string filePath, bool useShellExecute = default)
        {
            return Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = useShellExecute }).ThrowIfNull();
        }
    }
}