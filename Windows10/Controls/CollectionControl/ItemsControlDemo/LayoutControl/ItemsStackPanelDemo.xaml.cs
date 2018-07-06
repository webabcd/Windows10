/*
 * ItemsStackPanel - 虚拟化布局控件，ListView 的默认布局控件（继承自 Panel, 请参见 /Controls/LayoutControl/PanelDemo.xaml）
 *     FirstCacheIndex - 缓存中的第一项在全部数据中的索引位置
 *     FirstVisibleIndex - 屏幕上显示的第一项在全部数据中的索引位置
 *     LastCacheIndex - 缓存中的最后一项在全部数据中的索引位置
 *     LastVisibleIndex - 屏幕上显示的最后一项在全部数据中的索引位置
 *     CacheLength - 可见区外的需要缓存的数据的大小（以可见区条数大小的倍数为单位），默认值为 4.0
 *         比如当可见区可以显示 10 条数据，CacheLength 为 4 时，可见区外的需要缓存的数据的大小则为 4 * 10 = 40，也就是说整个缓存数据的大小为 10 + 4 * 10 = 50
 *         实际测试发现，可能会有一定的偏差，但是大体是准确的
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo.LayoutControl
{
    public sealed partial class ItemsStackPanelDemo : Page
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
        
        private ItemsStackPanel _itemsStackPanel1 = null;
        private ItemsStackPanel _itemsStackPanel2 = null;

        public ItemsStackPanelDemo()
        {
            this.InitializeComponent();

            this.Loaded += ItemsStackPanelDemo_Loaded;
        }

        private void ItemsStackPanelDemo_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.Zero;
            dTimer.Tick += DTimer_Tick;
            dTimer.Start();

            // 获取 ListView 中的 ItemsStackPanel 控件
            _itemsStackPanel1 = listView1.ItemsPanelRoot as ItemsStackPanel;
            _itemsStackPanel2 = listView2.ItemsPanelRoot as ItemsStackPanel;

            // 获取 ListView 中的 ItemsStackPanel 控件
            // _itemsStackPanel1 = Helper.GetVisualChild<ItemsStackPanel>(listView1);
            // _itemsStackPanel2 = Helper.GetVisualChild<ItemsStackPanel>(listView2);
        }

        private void DTimer_Tick(object sender, object e)
        {
            lblMsg1.Text = "FirstCacheIndex: " + _itemsStackPanel1.FirstCacheIndex.ToString();
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += "FirstVisibleIndex: " + _itemsStackPanel1.FirstVisibleIndex.ToString();
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += "LastCacheIndex: " + _itemsStackPanel1.LastCacheIndex.ToString();
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += "LastVisibleIndex: " + _itemsStackPanel1.LastVisibleIndex.ToString();
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += "CacheLength: " + _itemsStackPanel1.CacheLength.ToString();
        }

        private void cmbGroupHeaderPlacement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _itemsStackPanel2.GroupHeaderPlacement = (GroupHeaderPlacement)Enum.Parse(typeof(GroupHeaderPlacement), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
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
}
