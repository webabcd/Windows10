/*
 * Page - Page（继承自 UserControl, 请参见 /Controls/BaseControl/UserControlDemo/）
 *     TopAppBar, BottomAppBar - 参见 /Controls/NavigationControl/AppBarDemo.xaml 和 /Controls/NavigationControl/CommandBarDemo.xaml
 *     NavigationCacheMode, OnNavigatedFrom(), OnNavigatingFrom(), OnNavigatingFrom() - 参见 /Controls/NavigationControl/FrameDemo.xaml
 *     Frame - 获取当前 Page 的所属 Frame
 */

using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Controls.BaseControl.PageDemo
{
    public sealed partial class PageDemo : Page
    {
        public PageDemo()
        {
            this.InitializeComponent();

            this.Loading += page_Loading;
            this.Loaded += page_Loaded;
            this.Unloaded += page_Unloaded;
        }


        // 在 OnNavigatedTo 之后，由外到内触发 Loading 事件，由内到外触发 Loaded 事件；在 OnNavigatedFrom 之后，由外到内触发 Unloaded 事件（参见 /Controls/BaseControl/FrameworkElementDemo/Demo2.xaml.cs）
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            lblMsg.Text += "OnNavigatedTo";
            lblMsg.Text += Environment.NewLine;
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Debug.WriteLine("OnNavigatingFrom");
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Debug.WriteLine("OnNavigatedFrom");
        }


        // 在 OnNavigatedTo 之后，由外到内触发 Loading 事件，由内到外触发 Loaded 事件；在 OnNavigatedFrom 之后，由外到内触发 Unloaded 事件（参见 /Controls/BaseControl/FrameworkElementDemo/Demo2.xaml.cs）
        private void page_Loading(FrameworkElement sender, object args)
        {
            lblMsg.Text += "page_Loading";
            lblMsg.Text += Environment.NewLine;
        }
        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "page_Loaded";
            lblMsg.Text += Environment.NewLine;
        }
        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("page_Unloaded");
        }
    }
}
