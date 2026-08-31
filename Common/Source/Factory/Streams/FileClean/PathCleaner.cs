namespace Common.Source.Factory.Streams.FileClean
{
    public abstract class PathCleaner : IDisposable
    {
        private bool Disposed;

        private readonly bool IsFolder;

        public string FilePath { get; }

        public bool PlanToDelete { get; set; }

        protected PathCleaner(string filePath, bool planToDelete, bool isFolder)
        {
            FilePath = filePath;
            PlanToDelete = planToDelete;
            IsFolder = isFolder;
        }

        public void Delete()
        {
            if (Disposed)
            {
                return;
            }
            if (IsFolder)
            {
                Directory.Delete(FilePath);
            }
            else
            {
                File.Delete(FilePath);
            }
            Disposed = true;
        }

        public void Dispose()
        {
            if (PlanToDelete)
            {
                try { Delete(); } catch { }
            }
            GC.SuppressFinalize(this);
        }
    }
}