using System.Windows;
using System.Windows.Controls;

namespace UIDesign.Source.UI.Control
{
    public partial class Expander : UserControl
    {
        public Expander()
        {
            InitializeComponent();
        }

        private void ExpandClick(object sender, RoutedEventArgs e) => Dropdown = !Dropdown;

        public double PanelHeight
        {
            get => (double)GetValue(PanelHeightProperty);
            set => SetValue(PanelHeightProperty, value);
        }

        private static readonly DependencyProperty PanelHeightProperty = DependencyProperty.Register(nameof(PanelHeight), typeof(double), typeof(Expander));

        public bool Dropdown
        {
            get => (bool)GetValue(DropdownProperty);
            set => SetValue(DropdownProperty, value);
        }

        private static readonly DependencyProperty DropdownProperty = DependencyProperty.Register(nameof(Dropdown), typeof(bool), typeof(Expander));
    }
}