using System.Globalization;
using System.Windows.Data;

namespace StarRailDamage.Source.UI.Factory.Converter
{
    public class ObjectComparable : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((IComparable)value).CompareTo(parameter);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((IComparable)values[0]).CompareTo(values[1]);
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