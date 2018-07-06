/*
 * 演示如何对绑定的数据做自定义转换
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Windows10.Bind
{
    public sealed partial class BindingConverter : Page
    {
        public BindingConverter()
        {
            this.InitializeComponent();

            this.Loaded += BindingConverter_Loaded;
        }

        private void BindingConverter_Loaded(object sender, RoutedEventArgs e)
        {
            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
                ElementName = nameof(slider2),
                Path = new PropertyPath(nameof(Slider.Value)),
                Mode = BindingMode.TwoWay, // 默认是 OneWay 的
                Converter = new IntegerLetterConverter(),
                ConverterParameter = lblMsg, // 将 ConverterParameter 设置为一个指定的控件，这个在 xaml 中实现不了，但是可以在 C# 端实现
                ConverterLanguage = "zh"
            };

            // 将目标对象的目标属性与指定的 Binding 对象关联
            BindingOperations.SetBinding(textBox2, TextBox.TextProperty, binding);
        }
    }



    // 自定义一个实现了 IValueConverter 接口的类，用于对绑定的数据做自定义转换
    public sealed class IntegerLetterConverter : IValueConverter
    {
        /// <summary>
        /// 正向转换器。将值从数据源传给绑定目标时，数据绑定引擎会调用此方法
        /// </summary>
        /// <param name="value">转换之前的值</param>
        /// <param name="targetType">转换之后的数据类型</param>
        /// <param name="parameter">转换器所使用的参数（它是通过 Binding 的 ConverterParameter 传递过来的）</param>
        /// <param name="language">转换器所使用的区域信息（它是通过 Binding 的 ConverterLanguage 传递过来的）</param>
        /// <returns>转换后的值</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter != null && parameter.GetType() == typeof(TextBlock))
            {
                ((TextBlock)parameter).Text = $"value: {value}, targetType: {targetType}, parameter: {parameter}, language: {language}";
            }

            int v = (int)(double)value;
            return (char)v;
        }

        /// <summary>
        /// 反向转换器。将值从绑定目标传给数据源时，数据绑定引擎会调用此方法
        /// </summary>
        /// <param name="value">转换之前的值</param>
        /// <param name="targetType">转换之后的数据类型</param>
        /// <param name="parameter">转换器所使用的参数（它是通过 Binding 的 ConverterParameter 传递过来的）</param>
        /// <param name="language">转换器所使用的区域信息（它是通过 Binding 的 ConverterLanguage 传递过来的）</param>
        /// <returns>转换后的值</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (parameter != null && parameter.GetType() == typeof(TextBlock))
            {
                ((TextBlock)parameter).Text = $"value: {value}, targetType: {targetType}, parameter: {parameter}, language: {language}";
            }

            int v = ((string)value).ToCharArray()[0];
            return v;
        }
    }


    // 自定义一个实现了 IValueConverter 接口的类，用于格式化字符串
    public sealed class FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string format = (string)parameter;
            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}