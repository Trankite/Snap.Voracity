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
                SecurityIdentifier? user = WindowsIdentity.GetCurrent().User;
                if (user.IsNotNull())
                {
                    return user.AccountDomainSid.IsNotNull() ? user.AccountDomainSid.Value : user.Value;
                }
            }
            return $"{Environment.MachineName}-{Environment.UserName}";
        }

        static LocalSetting()
        {
            LocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppSetting.Developer);
        }
    }
}