/*
 * 本页为此 app 的导航页，同时用于演示：
 * 1、ItemsControl 控件如何分组
 * 2、SemanticZoom 控件的应用
 * 
 * 
 * SemanticZoom - 用于关联两个有语义关系的视图
 *     ZoomedInView - 放大后的视图，详细数据
 *     ZoomedOutView - 缩小后的视图，概述数据
 *     IsZoomedInViewActive - ZoomedInView 是否为当前正在显示的视图
 *     CanChangeViews - 是否可在两个视图间切换
 *     IsZoomOutButtonEnabled - 是否显示用于切换视图的按钮
 *     ToggleActiveView() - 切换视图
 *     ViewChangeStarted - 视图开始切换时触发的事件 
 *     ViewChangeCompleted - 视图切换完成时触发的事件
 *     
 * 
 * ItemsControl - 一个用于呈现集合数据的控件（GridView, ListView, FlipView, ListBox 等均直接或间接地继承了 ItemsControl）
 *     ItemsControl 控件提供了分组功能
 *     
 * CollectionViewSource - 对集合数据启用分组支持
 *     Source - 数据源
 *     View - 获取视图对象，返回一个实现了 ICollectionView 接口的对象
 *     IsSourceGrouped - 数据源是否是一个被分组的数据
 *     ItemsPath - 数据源中，子数据集合的属性名称
 *     
 * ICollectionView - 支持数据分组
 *     CollectionGroups - 组数据集合
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows10.Common;

namespace Windows10
{
    public sealed partial class Index : Page
    {
        // 本页所用到的 GridView 的垂直方向上的滚动值
        private double _scrollViewerVerticalOffset = 0;

        public Index()
        {
            this.InitializeComponent();

            XElement root = XElement.Load("SiteMap.xml");
            var items = ParseData(root);

            // 构造数据源
            CollectionViewSource groupedNavigationData = new CollectionViewSource();
            groupedNavigationData.IsSourceGrouped = true;
            groupedNavigationData.Source = items;
            groupedNavigationData.ItemsPath = new PropertyPath("Items");

            // 绑定数据
            gridViewDetails.ItemsSource = groupedNavigationData.View; // 全部数据
            gridViewSummary.ItemsSource = groupedNavigationData.View.CollectionGroups; // 关联的组数据集合

            // 必须缓存此页
            this.NavigationCacheMode = NavigationCacheMode.Required;

            gridViewDetails.Loaded += gridViewDetails_Loaded;
        }

        // 导航到其它页之前，保存本页所用到的 GridView 的垂直方向上的滚动值
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var scrollViewer = Helper.GetVisualChild<ScrollViewer>(gridViewDetails);
            _scrollViewerVerticalOffset = scrollViewer.VerticalOffset;
        }

        // GridView 加载之后，指定其垂直方向上的滚动值
        void gridViewDetails_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = Helper.GetVisualChild<ScrollViewer>(gridViewDetails);
            scrollViewer.ChangeView(0, _scrollViewerVerticalOffset, 1);
        }

        // 解析 xml 数据
        private List<NavigationModel> ParseData(XElement root)
        {
            if (root == null)
                return null;

            var items = from n in root.Elements("node")
                        select new NavigationModel
                        {
                            Title = (string)n.Attribute("title"),
                            Url = (string)n.Attribute("url"),
                            Items = ParseData(n)
                        };

            return items.ToList();
        }

        // 导航到指定的 Demo 页
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var model = (NavigationModel)(sender as Grid).Tag;

            MainPage.Current.SubTitle = model.Title;
            MainPage.Current.Container.Navigate(Type.GetType(model.Url));
        }


        class NavigationModel
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public List<NavigationModel> Items { get; set; }
        }
    }
}
