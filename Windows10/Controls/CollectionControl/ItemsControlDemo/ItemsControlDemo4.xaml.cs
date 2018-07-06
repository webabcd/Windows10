/*
 * ItemsControl - 集合控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     IsGrouping - 当前 ItemsControl 显示的是否是分组数据（只读）
 *     DependencyObject GroupHeaderContainerFromItemContainer(DependencyObject itemContainer) - 获取指定 ItemContainer 的 HeaderContainer
 *     
 *     
 * 本例用于演示如何通过 ItemsControl 显示分组数据
 * 
 * 注：本例是用 ListView 来演示数据分组的，用 GridView 做数据分组的示例请参见 /Index.xaml
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class ItemsControlDemo4 : Page
    {
        public CollectionViewSource MyData
        {
            get
            {
                XElement root = XElement.Load("SiteMap.xml");
                var items = LoadData(root);

                // 构造数据源
                CollectionViewSource source = new CollectionViewSource();
                source.IsSourceGrouped = true;
                source.Source = items;
                source.ItemsPath = new PropertyPath("Items");

                return source;
            }
        }

        public ItemsControlDemo4()
        {
            this.InitializeComponent();

            this.Loaded += ItemsControlDemo4_Loaded;
        }

        private void ItemsControlDemo4_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "IsGrouping: " + listView.IsGrouping.ToString();
        }

        private async void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && listView.ContainerFromItem(e.AddedItems[0]) != null)
            {
                // 获取选中数据的 HeaderContainer
                ListViewHeaderItem headerContainer = listView.GroupHeaderContainerFromItemContainer(listView.ContainerFromItem(e.AddedItems[0])) as ListViewHeaderItem;

                NavigationModel headerNavigationModel = headerContainer.Content as NavigationModel;
                await new MessageDialog($"header: {headerNavigationModel.Title}").ShowAsync();
            }
        }

        // 解析 xml 数据
        private List<NavigationModel> LoadData(XElement root)
        {
            if (root == null)
                return null;

            var items = from n in root.Elements("node")
                        select new NavigationModel
                        {
                            Title = (string)n.Attribute("title"),
                            Url = (string)n.Attribute("url"),
                            Items = LoadData(n)
                        };

            return items.ToList();
        }
    }

    // 自定义 MyGroupStyleSelector（GroupStyle 选择器）
    // 可以实现在 runtime 时，根据 group 的不同选择不同的 GroupStyle
    public class MyGroupStyleSelector : GroupStyleSelector
    {
        static bool temp = false;

        // GroupStyle 1（配置在 xaml 端）
        public GroupStyle GroupStyle1 { get; set; }

        // GroupStyle 2（配置在 xaml 端）
        public GroupStyle GroupStyle2 { get; set; }

        protected override GroupStyle SelectGroupStyleCore(object group, uint level)
        {
            // 我这里测试，group 要么是 null 要么是 DependencyObject，level 总是 0

            // 利用这个变量，来演示如何让不同的 group 使用不同的 GroupStyle
            temp ^= true;
            if (temp)
                return GroupStyle1;
            return GroupStyle2;

            // 如果想直接返回指定的资源也是可以的（但是不灵活），类似：return (GroupStyle)Application.Current.Resources["GroupStyle1"];
        }
    }
}