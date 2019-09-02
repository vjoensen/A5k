using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Noesis;

namespace A5k.UI
{
    class UppercasConverter
    {
        public class UppercaseConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return ((string)value).ToUpper();
            }
            public object ConvertBack(object value, Type targetType, object parameter,
                CultureInfo culture)
            {
                return null;
            }
        }
    }
}
