/*
 * 演示如何使用外部程序打开一个文件
 * 
 * Launcher - 用于启动与指定文件相关的应用程序
 *     LaunchFileAsync(IStorageFile file) - 打开指定的文件
 *     LaunchFileAsync(IStorageFile file, LauncherOptions options) - 打开指定的文件
 * 
 * LauncherOptions - 启动外部应用程序时的相关选项
 *     TreatAsUntrusted - 使用默认应用程序打开指定的文件时，是否弹出安全警告
 *     DisplayApplicationPicker - 是否弹出“打开方式”对话框
 *     UI.InvocationPoint - 指定“打开方式”对话框的显示位置
 *     
 * 当指定的文件不被任何应用程序支持时，可以用以下下两种方法处理
 * 1、指定 LauncherOptions.FallbackUri: 打开浏览器并跳转到指定的地址
 * 2、指定 LauncherOptions.PreferredApplicationDisplayName 和 LauncherOptions.PreferredApplicationPackageFamilyName
 *    PreferredApplicationDisplayName - 指定在弹出的“在商店搜索”对话框中所显示的应用程序名称
 *    PreferredApplicationPackageFamilyName - 用户点击“在商店搜索”后，会在商店搜索指定 PackageFamilyName
 */

using System;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.AssociationLaunching
{
    public sealed partial class LaunchFile : Page
    {
        public LaunchFile()
        {
            this.InitializeComponent();
        }

        private async void btnLaunchFile_Click(object sender, RoutedEventArgs e)
        {
            // 指定需要打开的文件
            StorageFile file = await Package.Current.InstalledLocation.GetFileAsync(@"Assets\hololens.jpg");

            // 指定打开文件过程中相关的各种选项
            LauncherOptions options = new LauncherOptions();
            if (radWarning.IsChecked.Value)
            {
                options.TreatAsUntrusted = true;
            }
            if (radOpenWith.IsChecked.Value)
            {
                Point openWithPosition = GetOpenWithPosition(btnLaunchFile);

                options.DisplayApplicationPicker = true;
                options.UI.InvocationPoint = openWithPosition;
            }

            // 使用外部程序打开指定的文件
            bool success = await Launcher.LaunchFileAsync(file, options);
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
