using StarRailDamage.Source.Extension;
using StarRailDamage.Source.UI.Factory.NotifyPropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.UI.Model.View
{
    public class TabulateItemModel : NotifyPropertyChangedFactory
    {
        private bool _Flag;

        private BitmapImage _Icon = BitmapImageExtension.DefaultImage;

        private string _Text = string.Empty;

        private string _Title = string.Empty;

        private ObservableCollection<string> _MarkItems = [];

        public bool Flag
        {
            get => _Flag;
            set => SetField(ref _Flag, value);
        }

        public BitmapImage Icon
        {
            get => _Icon;
            set => SetField(ref _Icon, value);
        }

        public string Text
        {
            get => _Text;
            set => SetField(ref _Text, value);
        }

        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        public ObservableCollection<string> MarkItems
        {
            get => _MarkItems;
            set => SetField(ref _MarkItems, value);
        }
    }
}