namespace Common.Source.Factory.Streams.FileClean
{
    public class FileCleaner : PathCleaner
    {
        private FileCleaner(string filePath, bool planToDelete) : base(filePath, planToDelete, false) { }

        public static FileCleaner Create(string filePath, bool planToDelete = default)
        {
            return new FileCleaner(filePath, planToDelete);
        }
    }
}