/*
 * 演示如何与 Indexer 绑定
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows10.Common;

namespace Windows10.Bind
{
    public sealed partial class BindingIndexer : Page
    {
        public BindingIndexer()
        {
            this.InitializeComponent();

            this.Loaded += BindingIndexer_Loaded;

            BindingDemo();
        }

        private void BindingIndexer_Loaded(object sender, RoutedEventArgs e)
        {
            // 用于演示如何绑定集合中的某个元素
            List<string> list = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                list.Add("索引：" + i.ToString());
            }
            textBlock.DataContext = list;

            // 用于演示如何绑定集合中的某个对象的某个属性
            textBlock2.DataContext = TestData.GetEmployees();

            // 用于演示如何绑定 string 类型的索引器
            textBlock3.DataContext = this;

            // 用于演示如何绑定字典表中指定 key 的数据
            Dictionary<string, string> dic = new Dictionary<string, string>() { { "hello", "hello webabcd" } };
            textBlock4.DataContext = dic;
        }

        // 自定义一个索引器
        public object this[string indexer]
        {
            get
            {
                return "string: " + indexer;
            }
        }



        // 在 C# 端绑定索引器
        private void BindingDemo()
        {
            textBox.DataContext = this;

            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
               Path = new PropertyPath("[wanglei]")
            };
            
            // 将目标对象的目标属性与指定的 Binding 对象关联
            BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);

            /*
             * 注：经测试在 TextBox 做如上绑定是正常的。但是如果在 TextBlock 做如上绑定则运行时报错 Attempted to read or write protected memory. This is often an indication that other memory is corrupt. 不知道为什么
             */
        }
    }
}
