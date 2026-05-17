using StarRailDamage.Source.Extension;
using System.Globalization;
using System.Windows.Data;

namespace StarRailDamage.Source.UI.Factory.Converter
{
    public class DoubleParser : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DoubleExtension.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}