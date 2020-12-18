using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Mv.Ui.Converters
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var firstLetter = value.ToString().First().ToString();
            var temp = new DrawingVisual();
            var c = temp.RenderOpen();

            var typeface = new Typeface(new FontFamily("Microsoft YaHei"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            c.DrawRectangle(Brushes.BlueViolet, null, new Rect(new Size(32, 32)));
            c.DrawText(new FormattedText(firstLetter, culture, FlowDirection.LeftToRight, typeface, 16, Brushes.White), new Point(8, 5));
            c.Close();
            var image = new DrawingImage(temp.Drawing);
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    public class BoolColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; }
        public Color FalseColor { get; set; }
        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value != null && (bool)value;
            if (Invert)
                v = !v;
            return new SolidColorBrush(v ?  TrueColor : FalseColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
