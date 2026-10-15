using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class TimeSpanExtension
    {
        [DebuggerStepThrough]
        public static string GetHourSecondString(this TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}