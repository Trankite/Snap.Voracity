namespace Common.Source.Factory.Streams.FileClean
{
    public class FolderCleaner : PathCleaner
    {
        private FolderCleaner(string filePath, bool planToDelete) : base(filePath, planToDelete, true) { }

        public static FolderCleaner Create(string filePath, bool planToDelete = default)
        {
            return new FolderCleaner(filePath, planToDelete);
        }
    }
}