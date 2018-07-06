/*
 * qualifiers - 限定符，在 uwp 中支持通过限定符来命名资源。限定符用于标识某个资源文件使用场景的上下文
 *
 * 本例用于演示如何获取系统支持的全部限定符
 * 关于限定符的规则及示例参见 /Resource/Qualifiers/Demo.xaml
 */

using System;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Resource.Qualifiers
{
    public sealed partial class Summary : Page
    {
        public Summary()
        {
            this.InitializeComponent();

            this.Loaded += Summary_Loaded;
        }

        private void Summary_Loaded(object sender, RoutedEventArgs e)
        {
            // 列举出系统支持的全部限定符，及其对应的值
            IObservableMap<string, string> qualifiers = ResourceContext.GetForCurrentView().QualifierValues;
            foreach (var qualifier in qualifiers)
            {
                lblMsg.Text += string.Format("{0}: {1}", qualifier.Key, qualifier.Value);
                lblMsg.Text += Environment.NewLine;
            }
            lblMsg.Text += Environment.NewLine;

            // 常用的有：Scale, DeviceFamily, Language, TargetSize, 其他的都不常用

            // 获取当前的缩放比例
            string scale;
            ResourceContext.GetForCurrentView().QualifierValues.TryGetValue("Scale", out scale);
            lblMsg.Text += "缩放比例: " + scale;
            lblMsg.Text += Environment.NewLine;

            // 获取当前的缩放比例（Windows.Graphics.Display.ResolutionScale 枚举）
            lblMsg.Text += "ResolutionScale: " + DisplayInformation.GetForCurrentView().ResolutionScale;
            lblMsg.Text += Environment.NewLine;

            // 获取当前的设备类型
            string deviceFamily;
            ResourceContext.GetForCurrentView().QualifierValues.TryGetValue("DeviceFamily", out deviceFamily);
            lblMsg.Text += "设备类型: " + deviceFamily;
            lblMsg.Text += Environment.NewLine;

            // 获取当前的语言类型
            string language;
            ResourceContext.GetForCurrentView().QualifierValues.TryGetValue("Language", out language);
            lblMsg.Text += "语言类型: " + language;
            lblMsg.Text += Environment.NewLine;
        }
    }
}
