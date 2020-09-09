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
            else
            {
                var s = value.ToString();
                if(s.ToUpper().Contains("PASS")||s.ToUpper().Contains("OK"))
                {
                    return Brushes.LimeGreen;
                }
                else
                {
                    return Brushes.Red;
                }
            }
          //  throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
