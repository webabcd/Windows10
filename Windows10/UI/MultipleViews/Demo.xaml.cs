/*
 * 演示“多窗口”相关知识点。本页是 PrimaryView
 *
 *
 * 解释一下本例中用于说明的几个名词：PrimaryView - 主窗口; SecondaryView - 新开窗口
 */

using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.UI.MultipleViews
{
    public sealed partial class Demo : Page
    {
        // 自定义的用于简化 SecondaryView 管理的帮助类
        SecondaryViewHelper _secondaryViewHelper = null;

        public Demo()
        {
            this.InitializeComponent();

            this.Loaded += Demo_Loaded;
        }

        private void Demo_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
        }

        private async void btnShow_Click(object sender, RoutedEventArgs e)
        {
            /*
             * CoreApplication.CreateNewView() - 创建一个新的 SecondaryView（只是新建一个 SecondaryView 实例，并不会显示出来）
             */
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _secondaryViewHelper = SecondaryViewHelper.CreateForCurrentView();
                _secondaryViewHelper.Title = "i am secondary view";

                _secondaryViewHelper.StartViewInUse();

                var frame = new Frame();
                frame.Navigate(typeof(SecondaryViewPage), _secondaryViewHelper);
                Window.Current.Content = frame;
                Window.Current.Activate();

                // 这里通过 ApplicationView.GetForCurrentView() 获取到的是新开窗口的 ApplicationView 对象
                ApplicationView secondaryView = ApplicationView.GetForCurrentView();
            });

            try
            {
                _secondaryViewHelper.StartViewInUse();

                /*
                 * ApplicationViewSwitcher.TryShowAsStandaloneAsync() - 在当前窗口的相邻位置显示另一个窗口
                 */
                ApplicationViewSwitcher.DisableShowingMainViewOnActivation();
                var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync
                (
                    _secondaryViewHelper.Id, // 需要显示的 SecondaryView 的窗口 id
                    ViewSizePreference.Default, // 需要显示的 SecondaryView 的尺寸首选项（经测试，此参数无效）
                    ApplicationView.GetForCurrentView().Id, // 调用者的窗口 id
                    ViewSizePreference.Default // 调用者的尺寸首选项（经测试，此参数无效）
                );

                if (!viewShown)
                {
                    lblMsg.Text = "显示 SecondaryView 失败";
                }

                _secondaryViewHelper.StopViewInUse();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        // 修改最近一个被我打开的 SecondaryView 的 Title
        private void btnChangeLastSecondaryViewTitle_Click(object sender, RoutedEventArgs e)
        {
            if (_secondaryViewHelper != null && !_secondaryViewHelper.IsReleased)
                _secondaryViewHelper.Title = new Random().Next().ToString();
        }
    }
}