using Common.Source.Core.Interface;
using Common.Source.Service;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace Common.Source.Factory.Streams.FileOpen
{
    public class FileOpenStream : IExceptionCapture, IDisposable
    {
        private readonly bool LeaveOpen;

        [MemberNotNullWhen(false, nameof(Exception))]
        [MemberNotNullWhen(true, nameof(Stream), nameof(FileInfo))]
        public bool Success { get; }

        public Stream? Stream { get; }

        public FileInfo? FileInfo { get; }

        public string FullPath => FileInfo?.FullName ?? string.Empty;

        public ExceptionDispatchInfo? Exception { get; }

        public FileOpenStream() { }

        public FileOpenStream(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None, bool create = default, bool leaveOpen = default)
        {
            try
            {
                LeaveOpen = leaveOpen;
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

        [MemberNotNull(nameof(Stream), nameof(FileInfo))]
        public void ThrowIfFailed()
        {
            if (!Success)
            {
                Exception.Throw();
            }
        }

        public void Dispose()
        {
            if (!LeaveOpen)
            {
                Stream?.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Success ? string.Empty : Exception.SourceException.Message;
        }
    }
}