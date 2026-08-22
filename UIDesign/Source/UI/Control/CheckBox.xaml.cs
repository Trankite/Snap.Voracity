using Common.Source.Extension;
using System.Windows;
using System.Windows.Controls;

namespace UIDesign.Source.UI.Control
{
    public partial class CheckBox : UserControl
    {
        public CheckBox()
        {
            InitializeComponent();
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CheckChargedEvent, this).Configure(Flag = !Flag));

        public static readonly RoutedEvent CheckChargedEvent = EventManager.RegisterRoutedEvent(nameof(CheckChargedEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckBox));

        public event RoutedEventHandler CheckCharged
        {
            add => AddHandler(CheckChargedEvent, value);
            remove => RemoveHandler(CheckChargedEvent, value);
        }

        public bool Flag
        {
            get => (bool)GetValue(FlagProperty);
            set => SetValue(FlagProperty, value);
        }

        public static readonly DependencyProperty FlagProperty = DependencyProperty.Register(nameof(Flag), typeof(bool), typeof(CheckBox));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(CheckBox), new PropertyMetadata(OnTextCharged));

        private static void OnTextCharged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CheckBox)d).Padding = new Thickness(string.IsNullOrEmpty(e.NewValue as string) ? 0 : 5, 0, 0, 0);
        }
    }
}