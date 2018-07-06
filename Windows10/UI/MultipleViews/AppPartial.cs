/*
 * 扩展 Application 对象，定义一些需要用到的全局变量
 */

using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10
{
    public partial class App
    {
        // PrimaryView 的 CoreDispatcher
        private CoreDispatcher _mainDispatcher;
        // PrimaryView 的窗口标识
        private int _mainViewId;

        // partial method，实现了 App.xaml.cs 中的声明
        partial void OnLaunched_MultipleViews(LaunchActivatedEventArgs args)
        {
            _mainDispatcher = Window.Current.Dispatcher;
            _mainViewId = ApplicationView.GetForCurrentView().Id;
        }
        
        public CoreDispatcher MainDispatcher
        {
            get
            {
                return _mainDispatcher;
            }
        }
        
        public int MainViewId
        {
            get
            {
                return _mainViewId;
            }
        }
    }
}
