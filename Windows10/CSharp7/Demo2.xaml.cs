/*
 * C# 7 示例 2
 * 值类型变量的引用和值类型返回值的引用, 模式匹配, 元组
 */

using System;
using Windows.UI.Xaml.Controls;

namespace Windows10.CSharp7
{
    public sealed partial class Demo2 : Page
    {
        public Demo2()
        {
            this.InitializeComponent();

            sample1();
            sample2();
            sample3();
        }


        // 值类型变量的引用和值类型返回值的引用（ref locals and returns）
        private void sample1()
        {
            // 值类型变量变为引用类型的示例
            int a = 1;
            ref int b = ref a; // 值类型变量 b 引用了值类型变量 a
            a = 2;
            lblMsg.Text = a.ToString();
            lblMsg.Text += Environment.NewLine;

            // 值类型返回值变为引用类型的示例
            int[] array = { 1, 2, 3, 4, 5 };
            ref int x = ref GetByIndex(array, 2); // 值类型变量 x 引用了 GetByIndex(array, 2)
            x = 99;
            lblMsg.Text += array[2].ToString();
            lblMsg.Text += Environment.NewLine;
        }
        // 返回的值类型变为引用类型
        private ref int GetByIndex(int[] array, int index)
        {
            return ref array[index];
        }


        // 模式匹配（pattern matching）
        private void sample2()
        {
            object a = 1;
            // 声明 int b，如果 a 是 int 类型则将 a 赋值给 b
            if (a is int b) 
            {
                lblMsg.Text += b.ToString();
                lblMsg.Text += Environment.NewLine;
            }
            
            switch (a)
            {
                // 声明 int c，如果 a 是 int 类型则将 a 赋值给 c，如果 c 大于 0 则执行此 case
                case int c when c > 0:
                    lblMsg.Text += "case int c when c > 0: " + c;
                    lblMsg.Text += Environment.NewLine;
                    break;
                // 声明 string c，如果 a 是 string 类型则将 a 赋值给 c
                case string c:
                    lblMsg.Text += "case string c: " + c;
                    lblMsg.Text += Environment.NewLine;
                    break;
            }
        }


        // 元组（Tuples）
        // 注：需要通过 nuget 引用 System.ValueTuple
        private void sample3()
        {
            // Tuples 特性是从 System.Tuple<T1, T2, T3...> 进化而来的
            // 注：当有多个返回值时，使用 Tuples 特性是非常方便的

            var user1 = GetUser1();
            lblMsg.Text += $"{user1.UserId}, {user1.UserName}, {user1.CreateTime}";
            lblMsg.Text += Environment.NewLine;

            var user2 = GetUser2();
            lblMsg.Text += $"{user2.UserId}, {user2.UserName}, {user2.CreateTime}";
            lblMsg.Text += Environment.NewLine;

            var user3 = GetUser3();
            lblMsg.Text += $"{user3.Item1}, {user3.Item2}, {user3.Item3}";
            lblMsg.Text += Environment.NewLine;

            (int UserId, string UserName, DateTime CreateTime) = GetUser1();
            lblMsg.Text += $"{UserId}, {UserName}, {CreateTime}";
            lblMsg.Text += Environment.NewLine;

            var obj1 = (UserId: 1, UserName: "webabcd");
            lblMsg.Text += $"{obj1.UserId}, {obj1.UserName}";
            lblMsg.Text += Environment.NewLine;

            var obj2 = (1, "webabcd");
            lblMsg.Text += $"{obj2.Item1}, {obj2.Item2}";
            lblMsg.Text += Environment.NewLine;

            (int id, string name) = (1, "webabcd");
            lblMsg.Text += $"{id}, {name}";
            lblMsg.Text += Environment.NewLine;
        }
        private (int UserId, string UserName, DateTime CreateTime) GetUser1()
        {
            return (1, "webabcd", DateTime.Now);
        }
        private (int UserId, string UserName, DateTime CreateTime) GetUser2()
        {
            return (UserId: 1, UserName: "webabcd", CreateTime: DateTime.Now);
        }
        private (int, string, DateTime) GetUser3()
        {
            return (1, "webabcd", DateTime.Now);
        }
    }
}
