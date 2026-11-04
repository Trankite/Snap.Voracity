using System.Windows;
using System.Windows.Media;

namespace UIDesign.Source.UI.Control
{
    public partial class TabControl : System.Windows.Controls.TabControl
    {
        public TabControl()
        {
            InitializeComponent();
        }

        private void TabItemCloseClick(object sender, RoutedEventArgs eventArguments)
        {
            if (!eventArguments.Handled)
            {
                RaiseEvent(new RoutedEventArgs(TabItemCloseEvent, sender));
            }
        }

        public void ClearEventBinding(object sender)
        {
            (sender as TabItem)?.TabItemClose -= TabItemCloseClick;
        }

        public static readonly RoutedEvent TabItemCloseEvent = EventManager.RegisterRoutedEvent(nameof(TabItemCloseEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabControl));

        public event RoutedEventHandler TabItemClose
        {
            add => AddHandler(TabItemCloseEvent, value);
            remove => RemoveHandler(TabItemCloseEvent, value);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            TabItem tabItem = new();
            tabItem.TabItemClose += TabItemCloseClick;
            return tabItem;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabItem;
        }

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register(nameof(FocusBrush), typeof(SolidColorBrush), typeof(TabControl));
    }
}