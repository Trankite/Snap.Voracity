using Common.Source.Extension;
using System.Globalization;
using System.Windows.Data;

namespace UIDesign.Source.Factory.Converter
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