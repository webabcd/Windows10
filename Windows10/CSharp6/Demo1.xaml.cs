/*
 * C# 6 示例 1
 * 自动属性支持初始化, 字符串嵌入的新方式, 通过 Using Static 引用静态类, nameof 表达式
 */

using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static System.Math; // 通过 Using Static 引用静态类

namespace Windows10.CSharp6
{
    public sealed partial class Demo1 : Page
    {
        // 自动属性支持初始化了
        public string MyName { get; set; } = "default value";
        // 只读自动属性也可以初始化
        public int MyAge { get; } = 17; 

        public Demo1()
        {
            this.InitializeComponent();

            this.Loaded += Demo1_Loaded;
        }

        private void Demo1_Loaded(object sender, RoutedEventArgs e)
        {
            sample1();
            sample2();
            sample3();
            sample4();
        }


        // 自动属性支持初始化（Initializers for auto-properties）
        private void sample1()
        {
            lblMsg.Text = this.MyName;
            lblMsg.Text += Environment.NewLine;

            lblMsg.Text += this.MyAge.ToString();
            lblMsg.Text += Environment.NewLine;
        }


        // 字符串嵌入（String Interpolation）的新方式
        private void sample2()
        {
            // 之前的字符串嵌入方式
            lblMsg.Text += string.Format("myName: {0}, myAge: {1}", this.MyName, this.MyAge);
            lblMsg.Text += Environment.NewLine;

            // 新的字符串嵌入方式
            lblMsg.Text += $"myName: {this.MyName}, myAge: {this.MyAge}, {{this.MyName}}";
            lblMsg.Text += Environment.NewLine;
        }


        // 通过 Using Static 引用静态类
        private void sample3()
        {
            // 之前通过 using static System.Math; 引用了静态类 System.Math
            // 那么之后就可以直接使用 System.Math 的方法了，如下
            lblMsg.Text += Abs(-100).ToString();
            lblMsg.Text += Environment.NewLine;
        }


        // nameof 表达式
        private void sample4()
        {
            DateTime dateTime = new DateTime();
            // nameof 表达式 - 用于获取变量的名称，比如下面这个会输出 "dateTime"，这个有什么用呢？参见之后的 "Book" 类的说明
            lblMsg.Text += nameof(dateTime);
            lblMsg.Text += Environment.NewLine;
        }
        // 演示 nameof 表达式的用途
        public class Book : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _title;
            public string Title
            {
                get { return _title; }
                set
                {
                    _title = value;

                    if (PropertyChanged != null)
                    {
                        // 这里以前只能这么写 PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                        // 现在可以向下面这样写
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Title)));
                        // 有什么用呢？
                        // 如果我要修改属性 Title 的名字时，而又忘了修改对应的 PropertyChangedEventArgs 中的名字，则编译会报错，以便修改
                        // 当然修改属性名字时最好用 Visual Studio 提供的“重命名”的方法
                    }
                }
            }
        }
    }
}
