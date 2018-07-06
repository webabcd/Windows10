/*
 * 演示如何开发自定义缓存文件更新程序
 * 
 * 1、在 Package.appxmanifest 中新增一个“缓存文件更新程序”声明，并做相关配置
 * 2、在 App.xaml.cs 中 override void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)，如果 app 是由文件打开选取器激活的，则可以在此获取其相关信息
 * 
 * CachedFileUpdaterActivatedEventArgs - 通过“缓存文件更新程序”激活应用程序时的事件参数
 *     CachedFileUpdaterUI - 获取 CachedFileUpdaterUI 对象
 *     PreviousExecutionState, Kind, SplashScreen - 各种激活 app 的方式的事件参数基
 * 
 * CachedFileUpdaterUI - 缓存文件更新程序的帮助类
 *     Title - 将在“缓存文件更新程序”上显示的标题
 *     UIStatus - “缓存文件更新程序”的 UI 状态（Unavailable, Hidden, Visible, Complete）
 *     UpdateTarget - Local 代表由 CachedFileUpdater 更新; Remote 代表由 app 更新
 *     UIRequested - 需要显示“缓存文件更新程序”的 UI 时触发的事件
 *     FileUpdateRequested - 当 app 激活缓存文件更新程序时，会触发 FileUpdateRequested 事件（事件参数：CachedFileUpdaterActivatedEventArgs）
 *     
 * CachedFileUpdaterActivatedEventArgs
 *     Request - 返回 FileUpdateRequest 类型的对象
 *     
 * FileUpdateRequest
 *     File - 关联的文件
 *     ContentId - 关联的文件标识
 *     Status - 文件的更新状态（FileUpdateStatus 枚举。Incomplete, Complete, UserInputNeeded, CurrentlyUnavailable, Failed, CompleteAndRenamed）
 *     GetDeferral() - 获取异步操作对象，同时开始异步操作，之后通过 Complete() 通知完成异步操作
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CachedFileUpdaterProvider
{
    public sealed partial class MyCachedFileUpdater : Page
    {
        private CachedFileUpdaterUI _cachedFileUpdaterUI;
        private FileUpdateRequest _fileUpdateRequest;
        private CoreDispatcher _dispatcher = Windows.UI.Xaml.Window.Current.Dispatcher;

        public MyCachedFileUpdater()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 CachedFileUpdaterUI 对象
            var args = (CachedFileUpdaterActivatedEventArgs)e.Parameter;
            _cachedFileUpdaterUI = args.CachedFileUpdaterUI;

            _cachedFileUpdaterUI.Title = "缓存文件更新程序";

            _cachedFileUpdaterUI.FileUpdateRequested += _cachedFileUpdaterUI_FileUpdateRequested;
            _cachedFileUpdaterUI.UIRequested += _cachedFileUpdaterUI_UIRequested;

            base.OnNavigatedTo(e);
        }

        // 需要显示 CachedFileUpdater 的 UI 时（即当 FileUpdateRequest.Status 等于 UserInputNeeded 时），会调用此事件处理器
        async void _cachedFileUpdaterUI_UIRequested(CachedFileUpdaterUI sender, object args)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Windows.UI.Xaml.Window.Current.Content = this;
                lblMsg.Text = "FileUpdateStatus: " + _fileUpdateRequest.Status.ToString();
            });
        }

        async void _cachedFileUpdaterUI_FileUpdateRequested(CachedFileUpdaterUI sender, FileUpdateRequestedEventArgs args)
        {
            _fileUpdateRequest = args.Request;
            FileUpdateRequestDeferral fileUpdateRequestDeferral = _fileUpdateRequest.GetDeferral();

            if (_cachedFileUpdaterUI.UpdateTarget == CachedFileTarget.Local) // 由 CachedFileUpdater 更新 CachedFile（CachedFileTarget.Local 方式）
            {
                // 开始激活 CachedFileUpdater 时，是 UIStatus.Hidden 状态的
                if (_cachedFileUpdaterUI.UIStatus == UIStatus.Hidden)
                {
                    // 下面针对两种方式分别写示例

                    // 方式一：直接更新文件，并设置为 FileUpdateStatus.Complete 状态，最后完成
                    if (DateTime.Now.Second % 2 == 0)
                    {
                        await FileIO.AppendTextAsync(_fileUpdateRequest.File, Environment.NewLine + "由 CachedFileUpdater 更新：" + DateTime.Now.ToString());
                        _fileUpdateRequest.Status = FileUpdateStatus.Complete;
                        fileUpdateRequestDeferral.Complete();
                    }
                    // 方式二：设置为 FileUpdateStatus.UserInputNeeded 状态，并完成，之后会再次触发这个事件，并且变为 UIStatus.Visible 状态，弹出本页界面，
                    // 这样的话可以在用户做一些操作之后再更新文件（参见下面的 btnUpdate_Click 部分）
                    else
                    {
                        _fileUpdateRequest.Status = FileUpdateStatus.UserInputNeeded;
                        fileUpdateRequestDeferral.Complete();
                    }
                }
            }
            else if (_cachedFileUpdaterUI.UpdateTarget == CachedFileTarget.Remote) // 由 app 更新 CachedFile（CachedFileTarget.Remote 方式）
            {
                // 这里可以拿到 app 更新后的文件的结果
                string textContent = await FileIO.ReadTextAsync(_fileUpdateRequest.File);
                // 但是这里不能修改这个文件，否则会报错
                // await FileIO.AppendTextAsync(_fileUpdateRequest.File, Environment.NewLine + "由 CachedFileUpdater 更新：" + DateTime.Now.ToString());

                // CachedFileUpdater 返回给 app 一个 FileUpdateStatus 状态
                _fileUpdateRequest.Status = FileUpdateStatus.Complete;
                fileUpdateRequestDeferral.Complete();
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            FileUpdateRequestDeferral fileUpdateRequestDeferral = _fileUpdateRequest.GetDeferral();

            // 由 CachedFileUpdater 更新 CachedFile，然后返回给 app 一个 FileUpdateStatus 状态
            await FileIO.AppendTextAsync(_fileUpdateRequest.File, Environment.NewLine + "由 CachedFileUpdater 更新：" + DateTime.Now.ToString());

            string fileContent = await FileIO.ReadTextAsync(_fileUpdateRequest.File);

            lblMsg.Text = "文件名: " + _fileUpdateRequest.File.Name;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "文件内容: " + fileContent;

            _fileUpdateRequest.Status = FileUpdateStatus.Complete;
            fileUpdateRequestDeferral.Complete();

            btnUpdate.IsEnabled = false;
        }
    }
}
