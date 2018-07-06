/*
 * 演示如何开发自定义文件打开选取器
 * 
 * 1、在 Package.appxmanifest 中新增一个“文件打开选取器”声明，并做相关配置
 * 2、在 App.xaml.cs 中 override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)，如果 app 是由文件打开选取器激活的，则会调用此方法
 * 
 * FileOpenPickerActivatedEventArgs - 通过“文件打开选取器”激活应用程序时的事件参数
 *     FileOpenPickerUI - 获取 FileOpenPickerUI 对象
 *     Kind - 此 app 被激活的类型（ActivationKind 枚举）
 *         比如，如果是通过“文件打开选取器”激活的话，则此值为 FileOpenPicker
 *     PreviousExecutionState - 此 app 被激活前的状态（ApplicationExecutionState 枚举）
 *         比如，如果此 app 被激活前就是运行状态的或，则此值为 Running
 *     SplashScreen - 获取此 app 的 SplashScreen 对象
 *     CallerPackageFamilyName - 获取激活了此 app 的应用的包名（但是实际测试发现，获取到的却是此 app 的包名）
 *     User - 获取激活了此 app 的 User 对象
 *     
 * FileOpenPickerUI - 自定义文件打开选取器的帮助类
 *     AllowedFileTypes - 允许的文件类型，只读
 *     SelectionMode - 选择模式（FileSelectionMode.Single 或 FileSelectionMode.Multiple）
 *     Title - 将在“自定义文件打开选取器”上显示的标题
 *     CanAddFile(IStorageFile file) - 是否可以将指定的文件添加进选中文件列表
 *     AddFile(string id, IStorageFile file) - 将文件添加进选中文件列表，并指定 id
 *     ContainsFile(string id) - 选中文件列表中是否包含指定的 id
 *     RemoveFile(string id) - 根据 id 从选中文件列表中删除对应的文件
 *     Closing - 用户关闭“自定义文件打开选取器”时触发的事件
 *     
 *     
 * 注意：测试时发现如果此 app 作为文件打开选取器激活之前是运行状态的话，则在作为文件打开选取器时会出现控件事件无法触发的情况（但是有的时候是正常的），不知道为什么，这一点开发和测试时要注意
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Picker
{
    public sealed partial class MyOpenPicker : Page
    {
        private FileOpenPickerUI _fileOpenPickerUI;

        public MyOpenPicker()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 FileOpenPickerUI 对象（从 App.xaml.cs 传来的）
            FileOpenPickerActivatedEventArgs args = (FileOpenPickerActivatedEventArgs)e.Parameter;
            _fileOpenPickerUI = args.FileOpenPickerUI;

            _fileOpenPickerUI.Title = "自定义文件打开选取器";

            // 注意：选择的文件的扩展名必须匹配 AllowedFileTypes 中的定义（其是在调用端的 FileOpenPicker.FileTypeFilter 中配置的）
            IReadOnlyList<string> allowedFileTypes = _fileOpenPickerUI.AllowedFileTypes;
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

            // _fileOpenPickerUI.Closing += _fileOpenPickerUI_Closing;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // _fileOpenPickerUI.Closing -= _fileOpenPickerUI_Closing;

            base.OnNavigatedFrom(e);
        }

        // 选择一个本地文件
        private async void btnPickLocalFile_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await Package.Current.InstalledLocation.GetFileAsync(@"Assets\hololens.jpg");
            if (_fileOpenPickerUI.CanAddFile(file))
            {
                AddFileResult result = _fileOpenPickerUI.AddFile("myFile", file);

                lblMsg.Text = "选择的文件: " + file.Name;
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "AddFileResult: " + result.ToString();
            }
        }

        // 选择一个远程文件
        private async void btnPickRemoteFile_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://images.cnblogs.com/mvpteam.gif", UriKind.Absolute);

            // 扩展名必须匹配 FileOpenPicker.FileTypeFilter 中的定义
            StorageFile file = await StorageFile.CreateStreamedFileFromUriAsync("mvp.gif", uri, RandomAccessStreamReference.CreateFromUri(uri));
            if (_fileOpenPickerUI.CanAddFile(file))
            {
                AddFileResult result = _fileOpenPickerUI.AddFile("myFile", file);

                lblMsg.Text = "选择的文件: " + file.Name;
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "AddFileResult: " + result.ToString();
            }
        }
    }
}
