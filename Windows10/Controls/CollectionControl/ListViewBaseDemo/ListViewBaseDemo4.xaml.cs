/*
 * ListViewBase(基类) - 列表控件基类（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 *     ContainerContentChanging - 数据虚拟化时，项容器的内容发生变化时触发的事件（仅 ItemsStackPanel 和 ItemsWrapGrid 有效）
 * 
 * 
 * 当 ListViewBase 的一屏需要显示的数据量极大时（一屏的 item 多，且每个 item 中的 element 也多），由于每次滚动时需要绘制当前屏的每个 element，这需要占用大量的 ui 资源，所以就会有一些卡顿
 * 为了解决这个问题 uwp 给出了两种解决方案
 * 1、设置 ListViewBase 的 ShowsScrollingPlaceholders 属性为 true（默认值），每次显示 item 时先显示占位符（尚不清楚怎么修改这个占位符的背景色），然后再绘制内容
 *    相关演示请参见：/Controls/CollectionControl/ListViewBaseDemo/ListViewBaseDemo1.xaml
 * 2、通过 ListViewBase 的 ContainerContentChanging 事件，分步绘制 item 中的 element
 *    本例即介绍这种方法。注意在 uwp 中已经不用这么麻烦了，可以通过 x:Bind 和 x:Phase 来实现，请参见：/Bind/PhaseDemo.xaml
 * 
 * 
 * 本例用于演示如何实现 ListViewBase 的分步绘制（大数据量流畅滚动）
 * 
 * 
 * 注：
 * 虚拟化布局控件用于减少创建的 item 数量
 * 分步绘制用于在绘制 item 时，分阶段绘制 item 上的元素
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ListViewBaseDemo
{
    public sealed partial class ListViewBaseDemo4 : Page
    {
        public ListViewBaseDemo4()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            gridView.ItemsSource = TestData.GetEmployees(1000);

            // 默认值是 true，即为了保证流畅，每次显示 item 时先会显示占位符（application 级的背景色块），然后再绘制内容
            // 本例演示 ContainerContentChanging 事件的使用，所以不会用到这个
            gridView.ShowsScrollingPlaceholders = false;
        }

        private void gridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            // 交由我处理吧（不用系统再处理了）
            args.Handled = true;

            // 第 1 阶段绘制
            // args.Phase.ToString(); // 0

            StackPanel templateRoot = (StackPanel)args.ItemContainer.ContentTemplateRoot;
            Rectangle placeholderRectangle = (Rectangle)templateRoot.FindName("placeholderRectangle");
            TextBlock lblName = (TextBlock)templateRoot.FindName("lblName");
            TextBlock lblAge = (TextBlock)templateRoot.FindName("lblAge");
            TextBlock lblIsMale = (TextBlock)templateRoot.FindName("lblIsMale");

            // 显示自定义占位符（也可以不用这个，而是直接显示 item 的背景）
            placeholderRectangle.Opacity = 1;

            // 除了占位符外，所有 item 全部暂时不绘制
            lblName.Opacity = 0;
            lblAge.Opacity = 0;
            lblIsMale.Opacity = 0;

            // 开始下一阶段的绘制
            args.RegisterUpdateCallback(ShowName);
        }

        private void ShowName(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            // 第 2 阶段绘制
            // args.Phase.ToString(); // 1

            Employee employee = (Employee)args.Item;
            SelectorItem itemContainer = (SelectorItem)args.ItemContainer;
            StackPanel templateRoot = (StackPanel)itemContainer.ContentTemplateRoot;
            TextBlock lblName = (TextBlock)templateRoot.FindName("lblName");

            // 绘制第 2 阶段的内容
            lblName.Text = employee.Name;
            lblName.Opacity = 1;

            // 开始下一阶段的绘制
            args.RegisterUpdateCallback(ShowAge);
        }

        private void ShowAge(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            // 第 3 阶段绘制
            // args.Phase.ToString(); // 2

            Employee employee = (Employee)args.Item;
            SelectorItem itemContainer = (SelectorItem)args.ItemContainer;
            StackPanel templateRoot = (StackPanel)itemContainer.ContentTemplateRoot;
            TextBlock lblAge = (TextBlock)templateRoot.FindName("lblAge");

            // 绘制第 3 阶段的内容
            lblAge.Text = employee.Age.ToString();
            lblAge.Opacity = 1;

            // 开始下一阶段的绘制
            args.RegisterUpdateCallback(ShowIsMale);
        }

        private void ShowIsMale(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            // 第 4 阶段绘制
            // args.Phase.ToString(); // 3

            Employee employee = (Employee)args.Item;
            SelectorItem itemContainer = (SelectorItem)args.ItemContainer;
            StackPanel templateRoot = (StackPanel)itemContainer.ContentTemplateRoot;
            Rectangle placeholderRectangle = (Rectangle)templateRoot.FindName("placeholderRectangle");
            TextBlock lblIsMale = (TextBlock)templateRoot.FindName("lblIsMale");

            // 绘制第 4 阶段的内容
            lblIsMale.Text = employee.IsMale.ToString();
            lblIsMale.Opacity = 1;

            // 隐藏自定义占位符
            placeholderRectangle.Opacity = 0;
        }
    }
}