using Common.Source.Core.Interface;
using Common.Source.Extension;
using Common.Source.Service;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace Common.Source.Factory.Streams.FileOpen
{
    public class FileOpenStream : IExceptionCapture, IDisposable
    {
        private readonly bool LeaveStreamOpen;

        [MemberNotNullWhen(false, nameof(CapturedException))]
        [MemberNotNullWhen(true, nameof(Stream), nameof(FileInfo))]
        public bool Success { get; }

        public Stream? Stream { get; }

        public FileInfo? FileInfo { get; }

        public string FullPath => FileInfo?.FullName ?? string.Empty;

        public ExceptionDispatchInfo? CapturedException { get; set; }

        public FileOpenStream() { }

        public FileOpenStream(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None, bool create = default, bool leaveOpen = default)
        {
            try
            {
                LeaveStreamOpen = leaveOpen;
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
                this.DispatchCapture(Exception);
            }
        }

        public static FileOpenStream Create(string path, FileMode fileMode = FileMode.Open, FileAccess fileAccess = FileAccess.ReadWrite, FileShare fileShare = FileShare.None, bool leaveOpen = default)
        {
            return new FileOpenStream(path, fileMode, fileAccess, fileShare, true, leaveOpen);
        }

        [MemberNotNull(nameof(Stream), nameof(FileInfo))]
        public void ThrowIfFailed()
        {
            if (!Success)
            {
                CapturedException.Throw();
            }
        }

        public void Dispose()
        {
            if (!LeaveStreamOpen)
            {
                Stream?.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return CapturedException.GetMessage();
        }
    }
}