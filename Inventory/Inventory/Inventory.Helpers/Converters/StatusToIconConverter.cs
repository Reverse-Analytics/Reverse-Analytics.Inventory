using System;
using System.Globalization;
using System.Windows.Data;

namespace Inventory.Helpers.Converters
{
    public class StatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value.ToString();

            return Core.Resources.IconsPath.PaymentRequired;

            if (status == "Closed")
            {

            }
            else if (status == "Due Soon")
            {

            }
            else if (status == "Overdue")
            {

            }
            else if (status == "Payment Required")
            {

            }
            else
            {

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
