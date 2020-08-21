using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Mv.Modules.Hmi.Converters
{
    public class ResultToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool b)
            {
                return b ? Brushes.LimeGreen : Brushes.Red;
            }
            if(value is string s)
            {
                if(s.ToUpper().Contains("PASS"))
                {
                    return Brushes.LimeGreen;
                }
                else
                {
                    return Brushes.Red;
                }
            }
            return Brushes.Black;
          //  throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
