using System.ComponentModel;
using System.Net.Http;
using System.Windows;

namespace StarRailDamage
{
    public class Program
    {
        public static bool DesignMode { get; }

        public static bool OnTerminal { get; set; }

        public static HttpClient HttpClient { get; }

        [STAThread]
        public static void Main() => App.Main();

        static Program()
        {
            HttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(15) };
            DesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
        }
    }
}