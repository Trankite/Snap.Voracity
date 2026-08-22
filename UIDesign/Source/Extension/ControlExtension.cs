using Common.Source.Extension;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace UIDesign.Source.Extension
{
    public static class ControlExtension
    {
        [DebuggerStepThrough]
        public static T SetFocusable<T>(this T control) where T : Control
        {
            return control.Configure(control.Focusable = true).Configure(control.FocusVisualStyle = default);
        }

        [DebuggerStepThrough]
        public static T SetSize<T>(this T control, double width, double height) where T : FrameworkElement
        {
            return control.Configure(control.Width = width).Configure(control.Height = height);
        }

        [DebuggerStepThrough]
        public static T SetCenterStartup<T>(this T window) where T : Window
        {
            return window.Configure(window.WindowStartupLocation = WindowStartupLocation.CenterScreen);
        }

        [DebuggerStepThrough]
        public static T SetTitle<T>(this T window, string title) where T : Window
        {
            return window.Configure(window.Title = title);
        }
    }
}