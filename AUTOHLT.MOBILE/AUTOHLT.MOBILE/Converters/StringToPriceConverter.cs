using System;
using System.Globalization;
using Xamarin.Forms;

namespace AUTOHLT.MOBILE.Converters
{
    public class StringToPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString() == "0")
                    return value;
                return string.Format(new CultureInfo("en-US"), "{0:0,0}", decimal.Parse(value.ToString()));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var res = value.ToString();
                if (res.Contains(","))
                    res = res.Replace(",", "");
                return res;
            }

            return value;
        }
    }
}