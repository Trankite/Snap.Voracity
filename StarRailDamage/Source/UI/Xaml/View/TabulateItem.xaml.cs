using StarRailDamage.Source.UI.Model.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.UI.Xaml.View
{
    public partial class TabulateItem : UserControl
    {
        public TabulateItem()
        {
            InitializeComponent();
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CheckChargedEvent, this));
        }

        public static readonly RoutedEvent CheckChargedEvent = EventManager.RegisterRoutedEvent(nameof(CheckChargedEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateItem));

        public event RoutedEventHandler CheckCharged
        {
            add => AddHandler(CheckChargedEvent, value);
            remove => RemoveHandler(CheckChargedEvent, value);
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ModifyEvent, this));
        }

        public static readonly RoutedEvent ModifyEvent = EventManager.RegisterRoutedEvent(nameof(ModifyEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateItem));

        public event RoutedEventHandler ModifyClick
        {
            add => AddHandler(ModifyEvent, value);
            remove => RemoveHandler(ModifyEvent, value);
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(DeleteEvent, this));

        public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent(nameof(DeleteEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateItem));

        public event RoutedEventHandler DeleteClick
        {
            add => AddHandler(DeleteEvent, value);
            remove => RemoveHandler(DeleteEvent, value);
        }

        public TabulateItemModel Model
        {
            get => (TabulateItemModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(TabulateItemModel), typeof(TabulateItem), new PropertyMetadata(new TabulateItemModel()));

        public bool Flag
        {
            get => (bool)GetValue(FlagProperty);
            set => SetValue(FlagProperty, value);
        }

        public static readonly DependencyProperty FlagProperty = DependencyProperty.Register(nameof(Flag), typeof(bool), typeof(TabulateItem), new PropertyMetadata(default(bool), FlagChangedCallback));

        private static void FlagChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabulateItem TabulateItem)
            {
                TabulateItem.Model.Flag = TabulateItem.Flag;
            }
        }

        public BitmapImage Icon
        {
            get => (BitmapImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(BitmapImage), typeof(TabulateItem), new PropertyMetadata(default(BitmapImage), IconChangedCallback));

        private static void IconChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabulateItem TabulateItem)
            {
                TabulateItem.Model.Icon = TabulateItem.Icon;
            }
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TabulateItem), new PropertyMetadata(default(string), TextChangedCallback));

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabulateItem TabulateItem)
            {
                TabulateItem.Model.Text = TabulateItem.Text;
            }
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(TabulateItem), new PropertyMetadata(default(string), TitleChangedCallback));

        private static void TitleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabulateItem TabulateItem)
            {
                TabulateItem.Model.Title = TabulateItem.Title;
            }
        }

        public ObservableCollection<string> MarkItems
        {
            get => (ObservableCollection<string>)GetValue(MarkItemsProperty);
            set => SetValue(MarkItemsProperty, value);
        }

        public static readonly DependencyProperty MarkItemsProperty = DependencyProperty.Register(nameof(MarkItems), typeof(ObservableCollection<string>), typeof(TabulateItem), new PropertyMetadata(default(ObservableCollection<string>), MarkItemsChangedCallback));

        private static void MarkItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabulateItem TabulateItem)
            {
                TabulateItem.Model.MarkItems = TabulateItem.MarkItems;
            }
        }

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(TabulateItem));

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register(nameof(FocusBrush), typeof(SolidColorBrush), typeof(TabulateItem));

        public SolidColorBrush TitleBrush
        {
            get => (SolidColorBrush)GetValue(TitleBrushProperty);
            set => SetValue(TitleBrushProperty, value);
        }

        public static readonly DependencyProperty TitleBrushProperty = DependencyProperty.Register(nameof(TitleBrush), typeof(SolidColorBrush), typeof(TabulateItem));
    }
}