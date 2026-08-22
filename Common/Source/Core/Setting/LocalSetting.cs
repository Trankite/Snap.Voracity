using Common.Source.Extension;
using System.Security.Principal;

namespace Common.Source.Core.Setting
{
    public static class LocalSetting
    {
        public static readonly string LocalPath;

        public static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), AppSetting.Developer);
        }

        public static string GetUserSid()
        {
            if (OperatingSystem.IsWindows())
            {
                SecurityIdentifier? User = WindowsIdentity.GetCurrent().User;
                return User.IsNotNull() ? User.ToString().LastSplit('-').Start.ToString() : Guid.NewGuid().ToString();
            }
            return string.Empty;
        }

        static LocalSetting()
        {
            LocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppSetting.Developer);
        }
    }
}