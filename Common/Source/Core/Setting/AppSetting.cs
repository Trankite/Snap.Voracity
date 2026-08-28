namespace Common.Source.Core.Setting
{
    public static class AppSetting
    {
        public const int BufferSize = 4 * 1024;

        public static bool OnTerminal { get; set; }

        public const string Developer = "Trankite";

        public static T[] GetBuffer<T>(int structSize)
        {
            return new T[BufferSize / structSize];
        }
    }
}