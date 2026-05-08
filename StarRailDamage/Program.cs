using StarRailDamage.Source.Service.Terminal;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;

namespace StarRailDamage
{
    public class Program
    {
        public static bool DesignMode { get; }

        public static bool PlanInitiation { get; set; }

        public static HttpClient HttpClient { get; }

        [STAThread]
        public static void Main(params string[] arguments)
        {
            TerminalHelper.Invoke(arguments);
            TerminalHelper.AllocMonitor().Wait();
            if (PlanInitiation)
            {
                App.Main();
            }
            ApplicationDispose();
        }

        public static void ApplicationDispose()
        {
            HttpClient.Dispose();
        }

        static Program()
        {
            PlanInitiation = true;
            HttpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(15) };
            DesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
        }
    }
}