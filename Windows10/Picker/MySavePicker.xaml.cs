/*
 * 演示如何开发自定义文件保存选取器
 * 
 * 1、在 Package.appxmanifest 中新增一个“文件保存选取器”声明，并做相关配置
 * 2、在 App.xaml.cs 中 override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)，如果 app 是由文件保存选取器激活的，则会调用此方法
 * 
 * FileSavePickerActivatedEventArgs - 通过“文件保存选取器”激活应用程序时的事件参数
 *     FileSavePickerUI - 获取 FileSavePickerUI 对象
 *     Kind - 此 app 被激活的类型（ActivationKind 枚举）
 *         比如，如果是通过“文件打开选取器”激活的话，则此值为 FileOpenPicker
 *     PreviousExecutionState - 此 app 被激活前的状态（ApplicationExecutionState 枚举）
 *         比如，如果此 app 被激活前就是运行状态的或，则此值为 Running
 *     SplashScreen - 获取此 app 的 SplashScreen 对象
 *     CallerPackageFamilyName - 获取激活了此 app 的应用的包名（但是实际测试发现，获取到的却是此 app 的包名）
 *     User - 获取激活了此 app 的 User 对象
 *     
 * FileSavePickerUI - 自定义文件保存选取器的帮助类
 *     AllowedFileTypes - 允许的文件类型，只读
 *     Title - 将在“自定义文件保存选取器”上显示的标题
 *     FileName - 需要保存的文件名（包括文件名和扩展名），只读
 *     TrySetFileName(string value) - 尝试指定需要保存的文件名（包括文件名和扩展名）
 *     FileNameChanged - 用户在文件名文本框中更改文件名或在文件类型下拉框中更改扩展名时触发的事件
 *     TargetFileRequested - 用户提交保存时触发的事件（事件参数：TargetFileRequestedEventArgs）
 *     
 * TargetFileRequestedEventArgs
 *     Request - 返回 TargetFileRequest 对象
 *     
 * TargetFileRequest
 *     TargetFile - 目标文件对象，用于返回给 client
 *     GetDeferral() - 获取异步操作对象，同时开始异步操作，之后通过 Complete() 通知完成异步操作
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Picker
{
    public sealed partial class MySavePicker : Page
    {
        private FileSavePickerUI _fileSavePickerUI;

        public MySavePicker()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 FileSavePickerUI 对象（从 App.xaml.cs 传来的）
            FileSavePickerActivatedEventArgs args = (FileSavePickerActivatedEventArgs)e.Parameter;
            _fileSavePickerUI = args.FileSavePickerUI;

            _fileSavePickerUI.Title = "自定义文件保存选取器";

            // 通过 AllowedFileTypes 获取到的允许的扩展名是在调用端的 FileSavePicker.FileTypeChoices 中配置的，实际保存扩展名可以不与其匹配
            IReadOnlyList<string> allowedFileTypes = _fileSavePickerUI.AllowedFileTypes;
            lblMsg.Text = "allowedFileTypes: " + string.Join(",", allowedFileTypes);
            lblMsg.Text += Environment.NewLine;

            lblMsg.Text += "Kind: " + args.Kind;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "SplashScreen.ImageLocation: " + args.SplashScreen.ImageLocation;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "PreviousExecutionState: " + args.PreviousExecutionState;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "CallerPackageFamilyName: " + args.CallerPackageFamilyName;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "User.NonRoamableId: " + args.User.NonRoamableId;
            lblMsg.Text += Environment.NewLine;

            _fileSavePickerUI.TargetFileRequested += _fileSavePickerUI_TargetFileRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _fileSavePickerUI.TargetFileRequested -= _fileSavePickerUI_TargetFileRequested;
        }

        private async void _fileSavePickerUI_TargetFileRequested(FileSavePickerUI sender, TargetFileRequestedEventArgs args)
        {
            // 异步操作
            var deferral = args.Request.GetDeferral();

            try
            {
                // 在指定的地址新建一个没有任何内容的空白文件
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(sender.FileName, CreationCollisionOption.GenerateUniqueName);

                // 设置 TargetFile，“自定义文件保存选取器”的调用端会收到此对象
                args.Request.TargetFile = file;
            }
            catch (Exception ex)
            {
                // 输出异常信息
                OutputMessage(ex.ToString());
            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }
        }

        private async void OutputMessage(string msg)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text = msg;
            });
        }
    }
}
