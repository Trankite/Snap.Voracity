namespace Common.Source.Factory.Streams.FileClean
{
    public class FileCleaner : PathCleaner
    {
        public FileCleaner(string path, bool finalize = default) : base(path, finalize) { }

        protected override void DeleteOverrid()
        {
            File.Delete(Path);
        }
    }
}