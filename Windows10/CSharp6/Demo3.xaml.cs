/*
 * C# 6 示例 3
 * 带索引的对象初始化器, null 值判断, lambda 表达式作用于字段或方法
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.CSharp6
{
    public sealed partial class Demo3 : Page
    {
        public Demo3()
        {
            this.InitializeComponent();

            this.Loaded += Demo3_Loaded;
        }

        private void Demo3_Loaded(object sender, RoutedEventArgs e)
        {
            sample1();
            sample2();
            sample3();
        }


        // 带索引的对象初始化器
        private void sample1()
        {
            // Dictionary 也可以这样初始化了
            var dict = new Dictionary<int, string>
            {
                [7] = "seven",
                [9] = "nine",
                [13] = "thirteen"
            };

            lblMsg.Text += dict[13].ToString();
            lblMsg.Text += Environment.NewLine;
        }


        // null 值判断
        private void sample2()
        {
            List<int> list = null;
            int? count = list?.Count; // 因为 list 是 null，所以 list?.Count 是 null
            int? value3 = list?[3]; // 因为 list 是 null，所以 list?[3] 是 null

            list = new List<int> { 1, 2, 3 };
            count = list?.Count;
            
            // 这句会异常的，因为 list 不是 null 且 list 没有第 11 个元素
            // int? value10 = list?[10];

            lblMsg.Text += count.ToString();
            lblMsg.Text += Environment.NewLine;

            // null 值判断的最主要的应用是这样的
            // 之前的写法
            object obj1 = null;
            if (obj1 != null)
            {
                obj1.ToString();
            }
            // 现在的写法
            object obj2 = null;
            obj2?.ToString();
        }


        // lambda 表达式作用于字段或方法
        private void sample3()
        {
            lblMsg.Text += this.ToString();
            lblMsg.Text += Environment.NewLine;

            lblMsg.Text += this.FullName;
            lblMsg.Text += Environment.NewLine;
        }

        public string FirstName { get; set; } = "lei";
        public string LastName { get; set; } = "wanglei";

        public override string ToString() => $"{FirstName} {LastName}"; // lambda 表达式作用于方法
        public string FullName => $"{FirstName} {LastName}"; // lambda 表达式作用于字段
    }
}
