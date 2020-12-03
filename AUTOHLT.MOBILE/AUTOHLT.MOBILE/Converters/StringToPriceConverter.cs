using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Converters
{
    public class StringToPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return string.Format(new CultureInfo("en-US"), "{0:0,0}", double.Parse(value.ToString()));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}