using Common.Source.Factory.Streams.Text.Interface;

namespace Common.Source.Factory.Streams.Text.Abstract
{
    public static class TextStreamSource
    {
        public static TextStreamSource<TReader, ITextWriter> Create<TReader>(TReader reader, TextWriter writer) where TReader : ITextReader
        {
            return Create<TReader>(reader, new TextWriterOwner(writer));
        }

        public static TextStreamSource<TReader, ITextWriter> Create<TReader>(TReader reader, ITextWriter writer) where TReader : ITextReader
        {
            return new TextStreamSource<TReader, ITextWriter>(reader, writer);
        }

        public static TextStreamSource<ITextReader, TWriter> Create<TWriter>(TextReader reader, TWriter writer) where TWriter : ITextWriter
        {
            return Create<TWriter>(new TextReaderOwner(reader), writer);
        }

        public static TextStreamSource<ITextReader, TWriter> Create<TWriter>(ITextReader reader, TWriter writer) where TWriter : ITextWriter
        {
            return new TextStreamSource<ITextReader, TWriter>(reader, writer);
        }
    }

    public class TextStreamSource<TReader, TWriter> : ITextStream where TReader : ITextReader where TWriter : ITextWriter
    {
        public TReader SourceReader { get; }

        public TWriter SourceWriter { get; }

        public ITextReader Reader => SourceReader;

        public ITextWriter Writer => SourceWriter;

        public TextStreamSource(TReader reader, TWriter writer)
        {
            SourceReader = reader;
            SourceWriter = writer;
        }

        public virtual void Dispose()
        {
            SourceWriter.Dispose();
            SourceReader.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}