/*
 * 演示如何使用外部程序打开指定的 Uri
 * 
 * Launcher - 用于启动与指定 Uri 相关的应用程序
 *     LaunchUriAsync(Uri uri) - 打开指定的 Uri
 *     LaunchUriAsync(Uri uri, LauncherOptions options) - 打开指定的 Uri
 *     LaunchUriForResultsAsync(Uri uri, LauncherOptions options, ValueSet inputData) - 打开指定的 Uri，并可以获取到被激活的 app 返回的数据（参见 App2AppCommunication/LaunchUriForResults.xaml.cs）
 * 
 * LauncherOptions - 启动外部应用程序时的相关选项
 *     TreatAsUntrusted - 使用默认应用程序打开指定的文件时，是否弹出安全警告
 *     DisplayApplicationPicker - 是否弹出“打开方式”对话框
 *     UI.InvocationPoint - 指定“打开方式”对话框的显示位置
 *     TargetApplicationPackageFamilyName - 如果要指定必须用某一目标程序打开的话，就指定此目标程序的 PackageFamilyName
 *     
 * 当指定的 Uri 不被任何应用程序支持时，可以用以下下两种方法处理
 * 1、指定 LauncherOptions.FallbackUri: 打开浏览器并跳转到指定的地址
 * 2、指定 LauncherOptions.PreferredApplicationDisplayName 和 LauncherOptions.PreferredApplicationPackageFamilyName
 *    PreferredApplicationDisplayName - 指定在弹出的“在商店搜索”对话框中所显示的应用程序名称
 *    PreferredApplicationPackageFamilyName - 用户点击“在商店搜索”后，会在商店搜索指定 PackageFamilyName
 */

using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.AssociationLaunching
{
    public sealed partial class LaunchUri : Page
    {
        public LaunchUri()
        {
            this.InitializeComponent();
        }

        private async void btnLaunchUri_Click(object sender, RoutedEventArgs e)
        {
            // 指定需要打开的 Uri
            Uri uri = new Uri("http://webabcd.cnblogs.com");

            // 指定打开 Uri 过程中相关的各种选项
            LauncherOptions options = new LauncherOptions();
            if (radWarning.IsChecked.Value)
            {
                options.TreatAsUntrusted = true;
            }
            if (radOpenWith.IsChecked.Value)
            {
                Point openWithPosition = GetOpenWithPosition(btnLaunchUri);

                options.DisplayApplicationPicker = true;
                options.UI.InvocationPoint = openWithPosition;
            }

            // 使用外部程序打开指定的 Uri
            bool success = await Launcher.LaunchUriAsync(uri, options);
            if (success)
            {
                lblMsg.Text = "打开成功";
            }
            else
            {
                lblMsg.Text = "打开失败";
            }
        }

        // 获取“打开方式”对话框的显示位置，即关联 Button 的左下角点的坐标
        private Point GetOpenWithPosition(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);

            Point desiredLocation = buttonTransform.TransformPoint(new Point());
            desiredLocation.Y = desiredLocation.Y + element.ActualHeight;

            return desiredLocation;
        }
    }
}
