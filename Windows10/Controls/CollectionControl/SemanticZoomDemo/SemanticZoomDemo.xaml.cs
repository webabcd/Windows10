/*
 * SemanticZoom - SemanticZoom 控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     ToggleActiveView() - 在 ZoomedInView, ZoomedOutView 两个视图之间切换
 *     ViewChangeStarted - 视图切换开始时触发的事件 
 *     ViewChangeCompleted - 视图切换完成时触发的事件
 *     
 * 
 * CollectionViewSource - 对集合数据启用分组支持
 *     Source - 数据源
 *     View - 获取视图对象，返回一个实现了 ICollectionView 接口的对象
 *     IsSourceGrouped - 数据源是否是一个被分组的数据
 *     ItemsPath - 数据源中，子数据集合的属性名称
 *     
 * ICollectionView - 支持数据分组是 ICollectionView 的作用之一
 *     CollectionGroups - 组数据集合
 */

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.SemanticZoomDemo
{
    public sealed partial class SemanticZoomDemo : Page
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

        public SemanticZoomDemo()
        {
            this.InitializeComponent();
        }

        private void btnToggleActiveView_Click(object sender, RoutedEventArgs e)
        {
            semanticZoom.ToggleActiveView();
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
