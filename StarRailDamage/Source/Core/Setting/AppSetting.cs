using StarRailDamage.Source.Extension;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

namespace StarRailDamage.Source.Core.Setting
{
    public static class AppSetting
    {
        public const string AppName = "StarRailDamage";

        public static readonly double PixelsPerDip;

        public static string GetUserSid()
        {
            SecurityIdentifier? User = WindowsIdentity.GetCurrent().User;
            return User.IsNotNull() ? User.ToString().LastSplit('-').Content.ToString() : Guid.NewGuid().ToString();
        }

        static AppSetting()
        {
            PixelsPerDip = !Program.DesignMode && Application.Current.IsNotNull() ? VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip : 1;
        }
    }
}