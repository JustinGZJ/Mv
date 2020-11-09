

namespace Mv.Ui.Converters
{
    public class NumberToDisplayDataSizeConverter : ValueConverterBase<double, DisplayDataSize>
    {
        protected override DisplayDataSize ConvertNonNullValue(double value) => value;
    }
}
