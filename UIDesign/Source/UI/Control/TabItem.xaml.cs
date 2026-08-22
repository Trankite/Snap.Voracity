using System.Windows;
using System.Windows.Media;

namespace UIDesign.Source.UI.Control
{
    public partial class TabItem : System.Windows.Controls.TabItem
    {
        public TabItem()
        {
            InitializeComponent();
        }

        private void TabItemCloseClick(object sender, RoutedEventArgs e)
        {
            if (!e.Handled) RaiseEvent(new RoutedEventArgs(TabItemCloseEvent, this));
        }

        public static readonly RoutedEvent TabItemCloseEvent = EventManager.RegisterRoutedEvent(nameof(TabItemCloseEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabItem));

        public event RoutedEventHandler TabItemClose
        {
            add => AddHandler(TabItemCloseEvent, value);
            remove => RemoveHandler(TabItemCloseEvent, value);
        }

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register(nameof(FocusBrush), typeof(SolidColorBrush), typeof(TabItem));
    }
}