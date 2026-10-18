using System.Diagnostics;

namespace Common.Source.Extension
{
    public static class TimeSpanExtension
    {
        [DebuggerStepThrough]
        public static string GetMinuteSecondString(this TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}";
        }

        [DebuggerStepThrough]
        public static string GetMinuteMicrosecondString(this TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalMinutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Microseconds:D3}";
        }

        [DebuggerStepThrough]
        public static string GetHourMinuteSecondString(this TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}