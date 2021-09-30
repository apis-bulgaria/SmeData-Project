using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmeData.Mobile.Converters
{
    public class StringToBoolenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string && !string.IsNullOrWhiteSpace((string)value))
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
