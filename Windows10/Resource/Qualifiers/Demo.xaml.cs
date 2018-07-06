/*
 * 本例用于演示限定符的实际效果
 */

using System;
using Windows.ApplicationModel.Resources.Core;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Resource.Qualifiers
{
    public sealed partial class Demo : Page
    {
        public Demo()
        {
            this.InitializeComponent();

            this.Loaded += Demo_Loaded;
        }

        private void Demo_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取当前的缩放比例
            string scale;
            ResourceContext.GetForCurrentView().QualifierValues.TryGetValue("Scale", out scale);
            lblMsg.Text += "缩放比例: " + scale;
            lblMsg.Text += Environment.NewLine;

            // 获取当前的缩放比例（Windows.Graphics.Display.ResolutionScale 枚举）
            lblMsg.Text += "ResolutionScale: " + DisplayInformation.GetForCurrentView().ResolutionScale.ToString();
            lblMsg.Text += Environment.NewLine;

            // 获取当前的设备类型
            string deviceFamily;
            ResourceContext.GetForCurrentView().QualifierValues.TryGetValue("DeviceFamily", out deviceFamily);
            lblMsg.Text += "设备类型: " + deviceFamily;
            lblMsg.Text += Environment.NewLine;
        }
    }
}
