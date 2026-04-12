using StarRailDamage.Source.Core.Abstraction;
using StarRailDamage.Source.Extension;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.ExceptionServices;

namespace StarRailDamage.Source.Service.FileOpen
{
    public class FileOpenStream : IExceptionCapture, IDisposable
    {
        [MemberNotNullWhen(true, nameof(Stream))]
        public bool Success { get; init; }

        public Stream? Stream { get; init; }

        public ExceptionDispatchInfo? Exception { get; set; }

        public FileOpenStream() { }

        public FileOpenStream(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite)
        {
            Success = StreamExtension.TryOpen(path, out FileStream? FileStream, fileMode, fileAccess, this) && true.Configure(Stream = FileStream);
        }

        public FileOpenStream(Stream stream)
        {
            Success = ObjectExtension.IsNotNull(Stream = stream);
        }

        public static FileOpenStream Create(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite)
        {
            return new FileOpenStream(FileHelper.BuildFilePath(path), fileMode, fileAccess);
        }

        public void Dispose()
        {
            Stream?.Dispose();
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Exception.IsNotNull() ? Exception.SourceException.Message : string.Empty;
        }
    }
}