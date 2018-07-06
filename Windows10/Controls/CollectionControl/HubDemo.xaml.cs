/*
 * Hub - Hub 控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     SectionsInView - 用于获取当前可视区中显示的全部 HubSection 集合
 *     SectionHeaders - 用于获取当前可视区中显示的全部 HubSection 对象的 Header 集合
 *        
 * HubSection - HubSection 控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *        
 *
 * 注：
 * Hub 实现了 ISemanticZoomInformation 接口，所以可以在 SemanticZoom 的两个视图间有关联地切换。关于 ISemanticZoomInformation 请参见 /Controls/CollectionControl/SemanticZoomDemo/ISemanticZoomInformationDemo.xaml
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.CollectionControl
{
    public sealed partial class HubDemo : Page
    {
        public HubDemo()
        {
            this.InitializeComponent();

            this.Loaded += HubDemo_Loaded;
        }

        private void HubDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // 拿到所有 HubSection 的 header 集合
            List<string> headers = new List<string>();
            foreach (HubSection section in hub.Sections)
            {
                if (section.Header != null)
                {

                    headers.Add(section.Header.ToString());
                }
            }
            
            listView.ItemsSource = headers;
        }

        private void hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            // 获取通过点击 HubSection 右上角的“查看更多”按钮而被选中的 HubSection 对象
            lblMsg.Text = "hub_SectionHeaderClick: " + e.Section.Header.ToString();
        }

        private void hub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            lblMsg.Text = "";

            // 此次在 hub 中移出的 HubSection
            if (e.RemovedSections.Count > 0)
            {
                lblMsg.Text += "hub_SectionsInViewChanged RemovedSections: " + e.RemovedSections[0].Header.ToString();
                lblMsg.Text += Environment.NewLine;
            }

            // 此次在 hub 中移入的 HubSection
            if (e.AddedSections.Count > 0)
            {
                lblMsg.Text += "hub_SectionsInViewChanged AddedSections: " + e.AddedSections[0].Header.ToString();
                lblMsg.Text += Environment.NewLine;
            }

            lblMsg.Text += "hub.SectionsInView: ";
            lblMsg.Text += Environment.NewLine;
            // 可视区中显示的全部 HubSection
            foreach (var item in hub.SectionsInView)
            {
                lblMsg.Text += item.Header.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }
    }
}
