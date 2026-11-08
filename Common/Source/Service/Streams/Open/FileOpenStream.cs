using Common.Source.Extension;
using Common.Source.Model.Interface;
using Common.Source.Service.Gradual.Abstract;
using Common.Source.Service.Gradual.Metadata;
using Common.Source.Service.Helper;
using System.Diagnostics.CodeAnalysis;

namespace Common.Source.Service.Streams.Open
{
    public class FileOpenStream : GradualTask, IExceptionThrowable, IDisposable
    {
        private bool Disposed;

        private readonly bool LeaveOpen;

        private readonly string FilePath;

        private readonly bool BuildFileDirectory;

        public FileStream? Stream { get; private set; }

        public FileInfo? FileInfo { get; private set; }

        public virtual FileMode FileMode { get; }

        public virtual FileAccess FileAccess { get; }

        public virtual FileShare FileShare { get; }

        [MemberNotNullWhen(true, nameof(Stream))]
        [MemberNotNullWhen(true, nameof(FileInfo))]
        public bool IsReady => Status.IsCompleted();

        protected FileOpenStream(string filePath, bool buildDirectory = default, bool leaveOpen = default)
        {
            FilePath = filePath;
            BuildFileDirectory = buildDirectory;
            LeaveOpen = leaveOpen;
        }

        public FileOpenStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, bool buildDirectory = default, bool leaveOpen = default) : this(filePath, buildDirectory, leaveOpen)
        {
            FileMode = fileMode;
            FileAccess = fileAccess;
            FileShare = fileShare;
        }

        public static FileOpenStream Create(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, bool buildDirectory = default, bool leaveOpen = default)
        {
            return new FileOpenStream(filePath, fileMode, fileAccess, fileShare, buildDirectory, leaveOpen).StartOrRetry();
        }

        protected override GradualStatus RetryOverride()
        {
            FileInfo = new FileInfo(FilePath);
            if (BuildFileDirectory)
            {
                FileHelper.BuildFilePath(FileInfo.FullName);
            }
            try
            {
                Stream = FileInfo.Open(FileMode, FileAccess, FileShare);
            }
            catch (Exception exception)
            {
                return FailedByException(exception, exception is not IOException);
            }
            return GradualStatus.Completed;
        }

        protected override void RefreshOverride()
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            FileInfo = default;
            Stream = DisposableHelper.Dispose(Stream);
        }

        [MemberNotNull(nameof(Stream))]
        [MemberNotNull(nameof(FileInfo))]
        public void ThrowIfFailed()
        {
            CapturedException?.Throw();
            ArgumentNullException.ThrowIfNull(Stream);
            ArgumentNullException.ThrowIfNull(FileInfo);
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                if (!LeaveOpen)
                {
                    Stream?.Dispose();
                }
                GC.SuppressFinalize(this);
                Disposed = true;
            }
        }

        public override string ToString()
        {
            return CapturedException.GetExceptionMessage();
        }
    }
}