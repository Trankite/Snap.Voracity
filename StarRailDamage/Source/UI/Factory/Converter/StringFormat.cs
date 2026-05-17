using StarRailDamage.Source.Extension;
using System.Globalization;
using System.Windows.Data;

namespace StarRailDamage.Source.UI.Factory.Converter
{
    public class StringFormat : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StringExtension.Format((string)parameter, value);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return StringExtension.Format((string)parameter, values);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}