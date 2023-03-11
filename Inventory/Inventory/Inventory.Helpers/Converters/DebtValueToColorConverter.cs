using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Inventory.Helpers.Converters
{
    public class DebtValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                return (decimal)value > 0 ? new SolidColorBrush(Color.FromRgb(235, 64, 52)) : new SolidColorBrush(Color.FromRgb(62, 62, 62));
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
