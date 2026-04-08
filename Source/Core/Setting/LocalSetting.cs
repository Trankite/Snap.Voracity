using System.IO;

namespace StarRailDamage.Source.Core.Setting
{
    public static class LocalSetting
    {
        public static readonly string LocalPath;

        public static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), AppSetting.AppName);
        }

        static LocalSetting()
        {
            LocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppSetting.AppName);
        }
    }
}