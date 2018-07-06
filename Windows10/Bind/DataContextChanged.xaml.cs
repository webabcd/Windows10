/*
 * DataContextChanged - FrameworkElement 的 DataContext 发生变化时触发的事件
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Bind
{
    public sealed partial class DataContextChanged : Page
    {
        public DataContextChanged()
        {
            this.InitializeComponent();

            this.Loaded += DataContextChanged_Loaded;
        }

        private void DataContextChanged_Loaded(object sender, RoutedEventArgs e)
        {
            // 指定数据上下文
            listBox.DataContext = new List<string> { "a", "b", "c" };
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            // 修改数据上下文
            listBox.DataContext = new List<string> { "a", "b", new Random().Next(0, 1000).ToString().PadLeft(3, '0') };
        }

        private void listBox_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            /*
             * FrameworkElement.DataContextChanged - 数据上下文发生改变后触发的事件
             */

            // 数据上下文发生改变后
            lblMsg.Text = "数据上下文发生改变：" + DateTime.Now.ToString("hh:mm:ss");

        }
    }
}
