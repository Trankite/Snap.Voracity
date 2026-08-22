using Common.Source.Extension;
using System.Globalization;
using System.Windows.Data;

namespace UIDesign.Source.Factory.Converter
{
    public class StringFormat : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StringExtension.SafeFormat((string)parameter, value);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return StringExtension.SafeFormat((string)parameter, values);
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