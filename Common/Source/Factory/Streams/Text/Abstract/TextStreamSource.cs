using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public static class TextStreamSource
    {
        public static TextStreamSource<TReader, ITextWriter> Create<TReader>(TReader reader, TextWriter writer, bool leaveOpen = default) where TReader : ITextReader
        {
            return Create<TReader>(reader, new TextWriterOwner(writer, leaveOpen), leaveOpen);
        }

        public static TextStreamSource<TReader, ITextWriter> Create<TReader>(TReader reader, ITextWriter writer, bool leaveOpen = default) where TReader : ITextReader
        {
            return new TextStreamSource<TReader, ITextWriter>(reader, writer, leaveOpen);
        }

        public static TextStreamSource<ITextReader, TWriter> Create<TWriter>(TextReader reader, TWriter writer, bool leaveOpen = default) where TWriter : ITextWriter
        {
            return Create<TWriter>(new TextReaderOwner(reader, leaveOpen), writer, leaveOpen);
        }

        public static TextStreamSource<ITextReader, TWriter> Create<TWriter>(ITextReader reader, TWriter writer, bool leaveOpen = default) where TWriter : ITextWriter
        {
            return new TextStreamSource<ITextReader, TWriter>(reader, writer, leaveOpen);
        }
    }

    public class TextStreamSource<TReader, TWriter> : ITextStream where TReader : ITextReader where TWriter : ITextWriter
    {
        private readonly bool LeaveStreamOpen;

        public TReader SourceReader { get; }

        public TWriter SourceWriter { get; }

        public ITextReader Reader => SourceReader;

        public ITextWriter Writer => SourceWriter;

        public TextStreamSource(TReader reader, TWriter writer, bool leaveOpen = default)
        {
            SourceReader = reader;
            SourceWriter = writer;
            LeaveStreamOpen = leaveOpen;
        }

        public virtual void Dispose()
        {
            if (!LeaveStreamOpen)
            {
                SourceWriter.Dispose();
                SourceReader.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}