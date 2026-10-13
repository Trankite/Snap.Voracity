using Common.Source.Extension;
using Common.Source.Factory.Streams.Text.Abstract;
using Common.Source.Factory.Streams.Text.Interface;
using System.Diagnostics;

namespace Common.Source.Factory.Streams.Text.Write
{
    public class StopTextWriter : TextWriterSource
    {
        public Stopwatch Watch { get; } = new Stopwatch();

        private readonly Func<TimeSpan, string> Formatter;

        public StopTextWriter(ITextWriter source, Func<TimeSpan, string>? formatter = default) : base(source)
        {
            Formatter = formatter ?? DefaultFormatter;
        }

        public StopTextWriter(ITextWriter source, Stopwatch watch, Func<TimeSpan, string>? formatter = default) : this(source, formatter)
        {
            Watch = watch;
        }

        private static string DefaultFormatter(TimeSpan timeSpan)
        {
            return $"[{timeSpan.GetHourSecondString()}]\x20";
        }

        public override void WriteLine(char value)
        {
            WriteTime();
            base.WriteLine(value);
        }

        public override void WriteLine(ReadOnlySpan<char> buffer)
        {
            WriteTime();
            base.WriteLine(buffer);
        }

        private void WriteTime()
        {
            Write(Formatter.Invoke(Watch.Elapsed));
        }
    }
}