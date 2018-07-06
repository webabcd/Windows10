/*
 * 演示“多窗口”相关知识点。本页是 SecondaryView
 */

using System;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UI.MultipleViews
{
    public sealed partial class SecondaryViewPage : Page
    {
        private SecondaryViewHelper _secondaryViewHelper;

        public SecondaryViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _secondaryViewHelper = (SecondaryViewHelper)e.Parameter;
            _secondaryViewHelper.Released += _secondaryViewHelper_Released;

            // 设置当前窗口的 Title
            ApplicationView.GetForCurrentView().Title = _secondaryViewHelper.Title;
            _secondaryViewHelper.PropertyChanged += _secondaryViewHelper_PropertyChanged;
        }

        private void _secondaryViewHelper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_secondaryViewHelper.Title))
            {
                _secondaryViewHelper.ApplicationView.Title = _secondaryViewHelper.Title;
            }
        }

        private async void _secondaryViewHelper_Released(object sender, EventArgs e)
        {
            ((SecondaryViewHelper)sender).Released -= _secondaryViewHelper_Released;

            await ((App)App.Current).MainDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // 调用主窗口线程，执行逻辑
            });

            Window.Current.Close();
        }

        private async void btnGoToMain_Click(object sender, RoutedEventArgs e)
        {
            _secondaryViewHelper.StartViewInUse();

            /*
             * ApplicationViewSwitcher.SwitchAsync() - 切换到指定的窗口
             */
            await ApplicationViewSwitcher.SwitchAsync
            (
                ((App)App.Current).MainViewId // 准备显示的窗口的 id
            );

            _secondaryViewHelper.StopViewInUse();
        }

        private async void btnGoToMainAndHideThisView_Click(object sender, RoutedEventArgs e)
        {
            _secondaryViewHelper.StartViewInUse();

            /*
             * ApplicationViewSwitcher.SwitchAsync() - 切换到指定的窗口
             */
            await ApplicationViewSwitcher.SwitchAsync
            (
                ((App)App.Current).MainViewId, // 准备显示的窗口的 id
                ApplicationView.GetForCurrentView().Id, // 调用者的窗口 id
                ApplicationViewSwitchingOptions.ConsolidateViews // 切换行为选项（Default - 标准动画切换; SkipAnimation - 不使用动画切换; ConsolidateViews - 切换后关闭调用者窗口）
            );

            _secondaryViewHelper.StopViewInUse();
        }
    }
}
