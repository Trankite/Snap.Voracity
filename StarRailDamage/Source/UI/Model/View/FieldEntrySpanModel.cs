using StarRailDamage.Source.UI.Factory.NotifyPropertyChanged;
using System.Windows.Media.Imaging;

namespace StarRailDamage.Source.UI.Model.View
{
    public class FieldEntrySpanModel : NotifyPropertyChangedFactory
    {
        private BitmapImage? _Icon;

        private string _Title = string.Empty;

        private string _Text = string.Empty;

        private string _Unit = string.Empty;

        private int _Digits;

        public FieldEntrySpanModel() { }

        public FieldEntrySpanModel(BitmapImage? icon, string title, string text, string unit, int digits)
        {
            _Icon = icon;
            _Title = title;
            _Text = text;
            _Unit = unit;
            _Digits = digits;
        }

        public BitmapImage? Icon
        {
            get => _Icon;
            set => SetField(ref _Icon, value);
        }

        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        public string Text
        {
            get => _Text;
            set => SetField(ref _Text, value);
        }

        public string Unit
        {
            get => _Unit;
            set => SetField(ref _Unit, value);
        }

        public int Digits
        {
            get => _Digits;
            set => SetField(ref _Digits, value);
        }
    }
}