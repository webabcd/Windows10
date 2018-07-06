using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Controls.NavigationControl
{
    public sealed partial class Frame1 : Page
    {
        public Frame1()
        {
            this.InitializeComponent();

            /*
             * Page.NavigationCacheMode - 使用 Frame 导航到此页面时，页面的缓存模式
             *     Disabled - 每次导航到页时，都重新实例化此页，默认值（Frame.CacheSize 无效）
             *     Enabled - 每次导航到页时，首先缓存此页，此时如果已缓存的页数大于 Frame.CacheSize，则按先进先出的原则丢弃最早的缓存页（Frame.CacheSize 有效）
             *     Required - 每次导航到页时，都缓存此页（Frame.CacheSize 无效）
             */
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            this.Loaded += Frame1_Loaded;
        }

        void Frame1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Loaded: " + DateTime.Now.ToString();
        }

        // 来了
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "OnNavigatedTo: " + DateTime.Now.ToString();
            lblMsg.Text += " param: " + (string)e.Parameter;
        }

        // 准备走了，但是可以取消
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "OnNavigatingFrom(NavigatingCancelEventArgs): " + DateTime.Now.ToString();
            lblMsg.Text += " param: " + (string)e.Parameter;
        }

        // 已经走了
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "OnNavigatedFrom(NavigationEventArgs): " + DateTime.Now.ToString();
            lblMsg.Text += " param: " + (string)e.Parameter;
        }
    }
}