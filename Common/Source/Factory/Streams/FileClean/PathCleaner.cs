namespace Common.Source.Factory.Streams.FileClean
{
    public abstract class PathCleaner : IDisposable
    {
        private bool Disposed;

        public string Path { get; }

        public bool Finalize { get; set; }

        protected abstract void DeleteOverrid();

        protected PathCleaner(string path, bool finalize)
        {
            Path = path;
            Finalize = finalize;
        }

        public void Delete()
        {
            if (!Disposed)
            {
                DeleteOverrid();
                Disposed = true;
            }
        }

        public void Dispose()
        {
            if (Finalize)
            {
                try { Delete(); } catch { }
            }
            GC.SuppressFinalize(this);
        }
    }
}