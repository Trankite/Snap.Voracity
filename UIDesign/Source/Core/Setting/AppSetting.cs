using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace UIDesign.Source.Core.Setting
{
    internal class AppSetting
    {
        public static bool IsDesignMode { get; }

        public static double PixelsPerDip { get; }

        static AppSetting()
        {
            IsDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
            PixelsPerDip = VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip;
        }
    }
}