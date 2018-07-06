using System;
using Windows.UI.Xaml.Data;

namespace Windows10.Common
{
    /// <summary>
    /// NullableBoolean 到 Boolean 的 Converter
    /// </summary>
    public class NullableBooleanToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool?)
            {
                return (bool)value;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return (bool)value;
            return false;
        }
    }
}
