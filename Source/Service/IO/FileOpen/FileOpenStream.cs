using StarRailDamage.Source.Core.Abstraction;
using StarRailDamage.Source.Extension;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.ExceptionServices;

namespace StarRailDamage.Source.Service.IO.FileOpen
{
    public class FileOpenStream : IExceptionCapture, IDisposable
    {
        [MemberNotNullWhen(true, nameof(Stream))]
        public bool Success { get; init; }

        public Stream? Stream { get; init; }

        public ExceptionDispatchInfo? Exception { get; set; }

        public FileOpenStream() { }

        public FileOpenStream(string path, FileMode fileMode = FileMode.Open)
        {
            Success = StreamExtension.TryOpen(path, out FileStream? _FileStream, fileMode, this) && true.Configure(Stream = _FileStream);
        }

        public FileOpenStream(Stream stream)
        {
            Success = ObjectExtension.IsNotNull(Stream = stream);
        }

        public void Dispose()
        {
            Stream?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}