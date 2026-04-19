using StarRailDamage.Source.UI.Factory.PropertyBinding;
using StarRailDamage.Source.UI.Model.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StarRailDamage.Source.UI.Xaml.View
{
    public partial class TabulateItem : UserControl
    {
        private static readonly PropertyBindingFactory<TabulateItem> BindingFactory = new();

        public TabulateItem()
        {
            InitializeComponent();
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            BindingFactory.ClearModelBinding(Model);
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CheckChargedEvent, this));

        public static readonly RoutedEvent CheckChargedEvent = EventManager.RegisterRoutedEvent(nameof(CheckChargedEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateItem));

        public event RoutedEventHandler CheckCharged
        {
            add => AddHandler(CheckChargedEvent, value);
            remove => RemoveHandler(CheckChargedEvent, value);
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ModifyEvent, this));

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

        public static readonly DependencyProperty ModelProperty = BindingFactory.ModelBinding(x => x.Model);

        public bool Flag
        {
            get => (bool)GetValue(FlagProperty);
            set => SetValue(FlagProperty, value);
        }

        public static readonly DependencyProperty FlagProperty = BindingFactory.DependBinding(x => x.Model.Flag, x => x.Flag, PropertyBindingMode.TwoWay);

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty = BindingFactory.DependBinding(x => x.Model.Icon, x => x.Icon);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = BindingFactory.DependBinding(x => x.Model.Text, x => x.Text);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty = BindingFactory.DependBinding(x => x.Model.Title, x => x.Title);

        public ObservableCollection<string> MarkItems
        {
            get => (ObservableCollection<string>)GetValue(MarkItemsProperty);
            set => SetValue(MarkItemsProperty, value);
        }

        public static readonly DependencyProperty MarkItemsProperty = BindingFactory.DependBinding(x => x.Model.MarkItems, x => x.MarkItems);

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty TitleFontSizeProperty = BindingFactory.DependProperty(x => x.TitleFontSize);

        public SolidColorBrush FocusBrush
        {
            get => (SolidColorBrush)GetValue(FocusBrushProperty);
            set => SetValue(FocusBrushProperty, value);
        }

        public static readonly DependencyProperty FocusBrushProperty = BindingFactory.DependProperty(x => x.FocusBrush);

        public SolidColorBrush TitleBrush
        {
            get => (SolidColorBrush)GetValue(TitleBrushProperty);
            set => SetValue(TitleBrushProperty, value);
        }

        public static readonly DependencyProperty TitleBrushProperty = BindingFactory.DependProperty(x => x.TitleBrush);
    }
}