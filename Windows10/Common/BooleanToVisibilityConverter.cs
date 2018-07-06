using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Windows10.Common
{
    /// <summary>
    /// 将 true 转换为 <see cref="Visibility.Visible"/> 并将 false 转换为 <see cref="Visibility.Collapsed"/> 的值转换器。
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
