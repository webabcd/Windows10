/*
 * 演示 SemanticZoom 如何与自定义的 ISemanticZoomInformation 类结合使用（本例开发了一个实现了 ISemanticZoomInformation 接口的自定义 FlipView，参见 MyFlipView.cs）
 * ZoomedInView 用自定义的 FlipView 演示，ZoomedOutView 用 GridView 演示
 * 
 * 
 * 注：
 * ListViewBase 实现了 ISemanticZoomInformation 接口，所以可以在 SemanticZoom 的两个视图间有关联地切换。如果想让其它控件也实现类似的功能，就必须使其实现 ISemanticZoomInformation 接口
 */

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.SemanticZoomDemo
{
    public sealed partial class ISemanticZoomInformationDemo : Page
    {
        public ISemanticZoomInformationDemo()
        {
            this.InitializeComponent();
            XElement root = XElement.Load("SiteMap.xml");
            var items = LoadData(root);

            // 绑定数据
            gridView.ItemsSource = items;
        }

        // 获取数据
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

        private void btnDisplayZoomedOutView_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            semanticZoom.IsZoomedInViewActive = false;
        }
    }
}