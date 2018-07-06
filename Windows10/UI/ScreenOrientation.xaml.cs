/*
 * 演示“屏幕方向”相关知识点
 */

using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UI
{
    public sealed partial class ScreenOrientation : Page
    {
        public ScreenOrientation()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 使用设备时的推荐方向，一般而言就是当“windows”键在下方时，设备的方向。手机一般是 Portrait，平板一般是 Landscape
            lblMsg.Text = "NativeOrientation: " + DisplayInformation.GetForCurrentView().NativeOrientation.ToString();
            lblMsg.Text += Environment.NewLine;

            // 设备的方向（Windows.Graphics.Display.DisplayOrientations 枚举：None, Landscape, Portrait, LandscapeFlipped, PortraitFlipped）
            // 注：LandscapeFlipped 是 Landscape 翻转了 180 度，PortraitFlipped 是 Portrait 翻转了 180 度
            // 注：Landscape 顺时针转（右转） 90 度是 Portrait，再顺时针转（右转） 90 度是 LandscapeFlipped
            lblMsg.Text += "CurrentOrientation: " + DisplayInformation.GetForCurrentView().CurrentOrientation.ToString();

            // NativeOrientation 或 CurrentOrientation 发生变化时触发的事件（NativeOrientation 一般是不会变的）
            DisplayInformation.GetForCurrentView().OrientationChanged += ScreenOrientation_OrientationChanged;
        }

        private void ScreenOrientation_OrientationChanged(DisplayInformation sender, object args)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "NativeOrientation: " + DisplayInformation.GetForCurrentView().NativeOrientation.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "CurrentOrientation: " + DisplayInformation.GetForCurrentView().CurrentOrientation.ToString();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DisplayInformation.GetForCurrentView().OrientationChanged -= ScreenOrientation_OrientationChanged;
        }

        private void btnLock_Checked(object sender, RoutedEventArgs e)
        {
            /* 在 Package.appxmanifest 中可以配置 app 的允许方向，类似如下（如果不配置就是允许任何方向）
               <uap:InitialRotationPreference>
                   <uap:Rotation Preference="portrait" />
                   <uap:Rotation Preference="landscape" />
                   <uap:Rotation Preference="portraitFlipped" />
                   <uap:Rotation Preference="landscapeFlipped" />
               <uap:InitialRotationPreference>
            */

            // DisplayInformation.AutoRotationPreferences - 指定当前 app 的首选方向，即强制通过指定的方向显示（必须是在 Package.appxmanifest 配置的允许方向之一）
            DisplayInformation.AutoRotationPreferences = DisplayInformation.GetForCurrentView().CurrentOrientation;
            btnLock.Content = "解除方向锁定";
        }

        private void btnLock_Unchecked(object sender, RoutedEventArgs e)
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
            btnLock.Content = "锁定当前方向";
        }
    }
}
