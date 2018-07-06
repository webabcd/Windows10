/*
 * OrientedVirtualizingPanel(基类) - VirtualizingStackPanel 和 WrapGrid 的基类
 *     ExtentWidth - OrientedVirtualizingPanel 内的内容的宽
 *     ExtentHeight - OrientedVirtualizingPanel 内的内容的高（本例测试结果为: 内容的总条数）
 *     ViewportWidth - 可视区的宽
 *     ViewportHeight - 可视区的高（本例测试结果为: 可视区的条数）
 *     HorizontalOffset - 滚动内容的水平方向的偏移量
 *     VerticalOffset - 滚动内容的垂直方向的偏移量（本例测试结果为: 偏移的数据条数）
 *     
 *     
 * 注：
 * 1、OrientedVirtualizingPanel 继承自 VirtualizingPanel（如果要写自定义的虚拟化布局控件，就继承这个类）
 * 2、VirtualizingPanel 继承自 Panel, 请参见 /Controls/LayoutControl/PanelDemo.xaml）
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo.LayoutControl
{
    public sealed partial class OrientedVirtualizingPanelDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(1000);

        private OrientedVirtualizingPanel _orientedVirtualizingPanel = null;

        public OrientedVirtualizingPanelDemo()
        {
            this.InitializeComponent();

            this.Loaded += OrientedVirtualizingPanelDemo_Loaded;
        }

        private void OrientedVirtualizingPanelDemo_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.Zero;
            dTimer.Tick += DTimer_Tick;
            dTimer.Start();

            // 获取 ListBox 中的 OrientedVirtualizingPanel 控件
            _orientedVirtualizingPanel = listBox.ItemsPanelRoot as OrientedVirtualizingPanel;

            // 获取 ListBox 中的 OrientedVirtualizingPanel 控件
            // _orientedVirtualizingPanel = Helper.GetVisualChild<OrientedVirtualizingPanel>(listBox);
        }

        private void DTimer_Tick(object sender, object e)
        {
            lblMsg.Text = "ExtentWidth: " + _orientedVirtualizingPanel.ExtentWidth.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ExtentHeight: " + _orientedVirtualizingPanel.ExtentHeight.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ViewportWidth: " + _orientedVirtualizingPanel.ViewportWidth.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ViewportHeight: " + _orientedVirtualizingPanel.ViewportHeight.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "HorizontalOffset: " + _orientedVirtualizingPanel.HorizontalOffset.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "VerticalOffset: " + _orientedVirtualizingPanel.VerticalOffset.ToString();
        }
    }
}
