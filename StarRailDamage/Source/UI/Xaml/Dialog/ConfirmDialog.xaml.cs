using StarRailDamage.Source.UI.Xaml.Control.Panel;
using System.Windows;

namespace StarRailDamage.Source.UI.Xaml.Dialog
{
    public partial class ConfirmDialog : ScopedDialog
    {
        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(ConfirmDialog));

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(ConfirmDialog));
    }
}