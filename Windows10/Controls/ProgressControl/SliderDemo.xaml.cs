/*
 * Slider - 滑动条控件（继承自 RangeBase, 请参见 /Controls/ProgressControl/RangeBaseDemo.xaml）
 * Thumb - 可由用户拖动的控件（Slider 内的可拖动部分就是一个 Thumb 控件）
 */

using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Windows10.Controls.ProgressControl
{
    public sealed partial class SliderDemo : Page
    {
        public SliderDemo()
        {
            this.InitializeComponent();
        }
    }

    // 为 Slider 的 ThumbToolTipValueConverter 提供 Converter
    public sealed class MyThumbToolTipValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // 在 thumb 旁显示的当前值的后面加一个百分号
            return value + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
