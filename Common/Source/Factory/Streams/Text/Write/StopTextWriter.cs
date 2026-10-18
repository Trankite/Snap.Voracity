using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using System.Diagnostics;

namespace Common.Source.Factory.Streams.Text.Write
{
    public class StopTextWriter : TextWriterSource
    {
        private readonly Lock SyncLock = new();

        public Stopwatch Stopwatch { get; } = new Stopwatch();

        private readonly Func<TimeSpan, string> Formatter;

        public StopTextWriter(ITextWriter source, Func<TimeSpan, string>? formatter = default, bool leaveOpen = default) : base(source, leaveOpen)
        {
            Formatter = formatter ?? DefaultTimeFormat;
        }

        public StopTextWriter(ITextWriter source, Stopwatch stopwatch, Func<TimeSpan, string>? formatter = default, bool leaveOpen = default) : this(source, formatter, leaveOpen)
        {
            Stopwatch = stopwatch;
        }

        private static string DefaultTimeFormat(TimeSpan timeSpan)
        {
            return $"[{timeSpan.GetMinuteMicrosecondString()}]\x20";
        }

        public override void WriteLine()
        {
            lock (SyncLock)
            {
                WriteTime();
                base.WriteLine();
            }
        }

        public override void WriteLine(char value)
        {
            lock (SyncLock)
            {
                WriteTime();
                base.WriteLine(value);
            }
        }

        public override void WriteLine(ReadOnlySpan<char> buffer)
        {
            lock (SyncLock)
            {
                WriteTime();
                base.WriteLine(buffer);
            }
        }

        private void WriteTime()
        {
            Write(Formatter.Invoke(Stopwatch.Elapsed));
        }
    }
}