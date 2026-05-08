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
        [MemberNotNullWhen(true, nameof(FileInfo))]
        public bool Success { get; }

        public Stream? Stream { get; }

        public FileInfo? FileInfo { get; }

        public string FullPath => FileInfo?.FullName ?? string.Empty;

        public ExceptionDispatchInfo? Exception { get; }

        public FileOpenStream() { }

        public FileOpenStream(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None, bool create = false)
        {
            try
            {
                FileInfo = new FileInfo(path);
                if (create)
                {
                    FileHelper.BuildFilePath(FullPath);
                }
                Stream = FileInfo.Open(fileMode, fileAccess, fileShare);
                Success = true;
            }
            catch (Exception Exception)
            {
                this.Exception = ExceptionDispatchInfo.Capture(Exception);
            }
        }

        public static FileOpenStream Create(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None) => new(path, fileMode, fileAccess, fileShare, true);

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