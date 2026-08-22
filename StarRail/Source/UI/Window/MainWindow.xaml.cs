using StarRail.Source.UI.Page;
using System.Collections.ObjectModel;
using System.Windows;
using UIDesign.Source.UI.Control;

namespace StarRail.Source.UI.Window
{
    public partial class MainWindow : UIDesign.Source.UI.Panel.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TabItems = [new("首页", new PortalHomePage()), new("测试角色", new MockBattlePage())];
        }

        private void TabItemClose(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is not TabItem TabItem) return;
            if (TabItem.Content is TabItemModel TabItemModel)
            {
                if (TabItems.IndexOf(TabItemModel) == 0) return;
                if (e.Source is TabControl TabControl)
                {
                    TabControl.ClearEventBinding(TabItem);
                }
                TabItems.Remove(TabItemModel);
            }
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(MainWindow));

        public ObservableCollection<TabItemModel> TabItems
        {
            get => (ObservableCollection<TabItemModel>)GetValue(TabItemsProperty);
            set => SetValue(TabItemsProperty, value);
        }

        public static readonly DependencyProperty TabItemsProperty = DependencyProperty.Register(nameof(TabItems), typeof(ObservableCollection<TabItemModel>), typeof(MainWindow));
    }
}