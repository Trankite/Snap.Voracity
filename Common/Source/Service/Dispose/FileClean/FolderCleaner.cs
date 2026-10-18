namespace Common.Source.Service.Dispose.FileClean
{
    public class FolderCleaner : PathCleaner
    {
        public bool Recursive { get; set; }

        public FolderCleaner(string path, bool finalize = default, bool recursive = default) : base(path, finalize)
        {
            Recursive = recursive;
        }

        protected override void DeleteOverrid()
        {
            Directory.Delete(Path, Recursive);
        }
    }
}