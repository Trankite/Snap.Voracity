using Common.Source.Extension;
using Common.Source.Service;
using Common.Source.Service.Mission;
using Common.Source.Service.Mission.Gradual.Abstract;
using Common.Source.Service.Mission.Gradual.Metadata;

namespace Common.Source.Factory.Streams.FileOpen
{
    public class FileOpenStream : GradualTask, IDisposable
    {
        private bool Disposed;

        private FileInfo? _FileInfo;

        private FileStream? _Stream;

        private readonly string FilePath;

        private readonly bool LeaveStreamOpen;

        public virtual FileMode FileMode { get; }

        public virtual FileAccess FileAccess { get; }

        public virtual FileShare FileShare { get; }

        protected virtual bool BuildDirectory { get; }

        public string FullName => FileInfo.FullName;

        public FileStream Stream { get => _Stream ?? throw this.GetException(); }

        public FileInfo FileInfo { get => _FileInfo ?? throw this.GetException(); }

        public bool CanReadStream => States == GradualStates.Completed;

        protected FileOpenStream(string filePath, bool buildDirectory = default, bool leaveStreamOpen = default)
        {
            FilePath = filePath;
            BuildDirectory = buildDirectory;
            LeaveStreamOpen = leaveStreamOpen;
        }

        public FileOpenStream(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, bool buildDirectory = default, bool leaveStreamOpen = default) : this(filePath, buildDirectory, leaveStreamOpen)
        {
            FileMode = fileMode;
            FileAccess = fileAccess;
            FileShare = fileShare;
        }

        public static FileOpenStream Create(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare, bool buildDirectory = default, bool leaveStreamOpen = default)
        {
            return new FileOpenStream(filePath, fileMode, fileAccess, fileShare, buildDirectory, leaveStreamOpen).Configure(Self => Self.StartOrRetry());
        }

        protected override GradualStates RetryOverride()
        {
            _FileInfo = new FileInfo(FilePath);
            if (BuildDirectory)
            {
                FileHelper.BuildFilePath(FullName);
            }
            _Stream = FileInfo.Open(FileMode, FileAccess, FileShare);
            return GradualStates.Completed;
        }

        protected override void RefreshOverride()
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            _FileInfo = default;
            DisposableHelper.DisposeAndSetNull(ref _Stream);
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                if (!LeaveStreamOpen)
                {
                    _Stream?.Dispose();
                }
                GC.SuppressFinalize(this);
                Disposed = true;
            }
        }

        public override string ToString()
        {
            return CapturedException.GetMessage();
        }
    }
}