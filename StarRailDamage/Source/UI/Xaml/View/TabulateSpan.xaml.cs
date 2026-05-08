using StarRailDamage.Source.Extension;
using StarRailDamage.Source.UI.Model.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StarRailDamage.Source.UI.Xaml.View
{
    public partial class TabulateSpan : UserControl
    {
        public TabulateSpan()
        {
            InitializeComponent();
        }

        private void ItemDropdown(object sender, MouseButtonEventArgs e) => Dropdown = true;

        private void ItemSelected(object sender, MouseButtonEventArgs e)
        {
            Select = ((TabulateItem)sender).Model.Configure(Dropdown = false);
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(CheckChargedEvent, sender));

        public static readonly RoutedEvent CheckChargedEvent = EventManager.RegisterRoutedEvent(nameof(CheckChargedEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateSpan));

        public event RoutedEventHandler CheckCharged
        {
            add => AddHandler(CheckChargedEvent, value);
            remove => RemoveHandler(CheckChargedEvent, value);
        }

        private void ModifyItemClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(ModifyEvent, sender));

        public static readonly RoutedEvent ModifyEvent = EventManager.RegisterRoutedEvent(nameof(ModifyEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateSpan));

        public event RoutedEventHandler ModifyClick
        {
            add => AddHandler(ModifyEvent, value);
            remove => RemoveHandler(ModifyEvent, value);
        }

        private void DeleteItemClick(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(DeleteEvent, sender));

        public static readonly RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent(nameof(DeleteEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabulateSpan));

        public event RoutedEventHandler DeleteClick
        {
            add => AddHandler(DeleteEvent, value);
            remove => RemoveHandler(DeleteEvent, value);
        }

        public ObservableCollection<TabulateItemModel> Items
        {
            get => (ObservableCollection<TabulateItemModel>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<TabulateItemModel>), typeof(TabulateSpan));

        public TabulateItemModel Select
        {
            get => (TabulateItemModel)GetValue(SelectProperty);
            set => SetValue(SelectProperty, value);
        }

        public static readonly DependencyProperty SelectProperty = DependencyProperty.Register(nameof(Select), typeof(TabulateItemModel), typeof(TabulateSpan));

        public bool Dropdown
        {
            get => (bool)GetValue(DropdownProperty);
            set => SetValue(DropdownProperty, value);
        }

        public static readonly DependencyProperty DropdownProperty = DependencyProperty.Register(nameof(Dropdown), typeof(bool), typeof(TabulateSpan));

        public double DropdownHeight
        {
            get => (double)GetValue(DropdownHeightProperty);
            set => SetValue(DropdownHeightProperty, value);
        }

        public static readonly DependencyProperty DropdownHeightProperty = DependencyProperty.Register(nameof(DropdownHeight), typeof(double), typeof(TabulateSpan));
    }
}