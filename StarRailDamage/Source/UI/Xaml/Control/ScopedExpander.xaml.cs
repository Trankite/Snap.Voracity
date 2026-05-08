using System.Windows;
using System.Windows.Controls;

namespace StarRailDamage.Source.UI.Xaml.Control
{
    public partial class ScopedExpander : UserControl
    {
        public ScopedExpander()
        {
            InitializeComponent();
        }

        private void ExpandClick(object sender, RoutedEventArgs e) => Dropdown = !Dropdown;

        public double PanelHeight
        {
            get => (double)GetValue(PanelHeightProperty);
            set => SetValue(PanelHeightProperty, value);
        }

        private static readonly DependencyProperty PanelHeightProperty = DependencyProperty.Register(nameof(PanelHeight), typeof(double), typeof(ScopedExpander));

        public bool Dropdown
        {
            get => (bool)GetValue(DropdownProperty);
            set => SetValue(DropdownProperty, value);
        }

        private static readonly DependencyProperty DropdownProperty = DependencyProperty.Register(nameof(Dropdown), typeof(bool), typeof(ScopedExpander));
    }
}