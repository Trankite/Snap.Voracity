using StarRailDamage.Source.UI.Control.Panel;
using System.Windows;

namespace StarRailDamage.Source.UI.Xaml.Window
{
    public partial class ConfirmWindow : ScopedWindow
    {
        public ConfirmWindow()
        {
            InitializeComponent();
        }

        public string WindowTitle
        {
            get => (string)GetValue(WindowTitleProperty);
            set => SetValue(WindowTitleProperty, value);
        }

        public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.Register(nameof(WindowTitle), typeof(string), typeof(ConfirmWindow));
    }
}