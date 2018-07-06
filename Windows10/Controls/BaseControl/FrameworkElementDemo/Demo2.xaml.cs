/*
 * FrameworkElement - FrameworkElement（继承自 UIElement, 请参见 /Controls/BaseControl/UIElementDemo/）
 *     DataContextChanged - 数据上下文发生改变后触发的事件（参见 /Bind/DataContextChanged.xaml）
 *     Loading - 在 OnNavigatedTo 之后，由外到内触发 Loading 事件
 *     Loaded - 由内到外触发 Loaded 事件
 *     Unloaded - 在 OnNavigatedFrom 之后，由外到内触发 Unloaded 事件
 *     SizeChanged - 在尺寸（ActualWidth 或 ActualHeight）发生变化时触发的事件，位置等发生变化时不会触发
 *     LayoutUpdated - 布局更新时触发的事件，比如尺寸发生了变化或位置发生了变化等
 *     
 *     
 * 本例用于演示 FrameworkElement 的相关事件
 */

using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Controls.BaseControl.FrameworkElementDemo
{
    public sealed partial class Demo2 : Page
    {
        public Demo2()
        {
            this.InitializeComponent();

            this.Loading += page_Loading;
            this.Loaded += page_Loaded;
            this.Unloaded += page_Unloaded;

            stackPanel.Loading += stackPanel_Loading;
            stackPanel.Loaded += stackPanel_Loaded;
            stackPanel.Unloaded += stackPanel_Unloaded;

            lblMsg.Loading += lblMsg_Loading;
            lblMsg.Loaded += lblMsg_Loaded;
            lblMsg.Unloaded += lblMsg_Unloaded;

            lblMsg.SizeChanged += lblMsg_SizeChanged;
            lblMsg.LayoutUpdated += lblMsg_LayoutUpdated;
        }



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



        private void stackPanel_Loading(FrameworkElement sender, object args)
        {
            lblMsg.Text += "stackPanel_Loading";
            lblMsg.Text += Environment.NewLine;
        }
        private void stackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "stackPanel_Loaded";
            lblMsg.Text += Environment.NewLine;
        }
        private void stackPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("stackPanel_Unloaded");
        }



        private void lblMsg_Loading(FrameworkElement sender, object args)
        {
            lblMsg.Text += "lblMsg_Loading";
            lblMsg.Text += Environment.NewLine;
        }
        private void lblMsg_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "lblMsg_Loaded";
            lblMsg.Text += Environment.NewLine;
        }
        private void lblMsg_Unloaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("lblMsg_Unloaded");
        }



        private void lblMsg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"lblMsg_SizeChanged, PreviousSize:{e.PreviousSize}, NewSize:{e.NewSize}");

            // 注：如果在这里又改变了 lblMsg 的尺寸的话，则会发生循环调用，编译时会出现“Layout cycle detected.  Layout could not complete.”错误
        }
        private void lblMsg_LayoutUpdated(object sender, object e)
        {
            Debug.WriteLine("lblMsg_LayoutUpdated");

            // 注：如果在这里又改变了 lblMsg 的布局的话，则会发生循环调用，编译时会出现“Layout cycle detected.  Layout could not complete.”错误
        }
    }
}
