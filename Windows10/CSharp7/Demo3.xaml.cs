/*
 * C# 7 示例 3
 * 表达式抛出异常, lambda 表达式作用于构造函数或属性, 局部函数
 */

using System;
using Windows.UI.Xaml.Controls;

namespace Windows10.CSharp7
{
    public sealed partial class Demo3 : Page
    {
        public Demo3()
        {
            this.InitializeComponent();

            sample1();
            sample2();
            sample3();
        }


        // 表达式抛出异常（throw expressions）
        private void sample1()
        {
            try
            {
                string a = null;
                // 支持在表达式中抛出异常
                string b = a ?? throw new Exception("ex");
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }


        // lambda 表达式作用于构造函数或属性（more expression-bodied members）
        // 注：在 c#6 中已经支持了 lambda 表达式作用于字段或方法
        private void sample2()
        {
            MyClass obj = new MyClass("webabcd");
            lblMsg.Text += obj.Text;
            lblMsg.Text += Environment.NewLine;
        }
        public class MyClass
        {
            private string _text;

            public MyClass(string text) => _text = text; // lambda 表达式作用于构造函数

            public string Text // lambda 表达式作用于属性
            {
                get => _text;
                set => _text = value ?? "default text";
            }
        }


        // 局部函数（local functions）
        private void sample3()
        {
            int a = GetNumber();
            lblMsg.Text += a.ToString();
            lblMsg.Text += Environment.NewLine;

            // 支持局部函数了
            int GetNumber()
            {
                return 1;
            }
        }
    }
}
