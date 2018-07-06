/*
 * C# 7 示例 1
 * out 变量, 数字语法改进, 值类型的异步返回
 */

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Windows10.CSharp7
{
    public sealed partial class Demo1 : Page
    {
        public Demo1()
        {
            this.InitializeComponent();

            sample1();
            sample2();
            sample3();
        }


        // out 变量（out-variables）
        private void sample1()
        {
            // 这是之前的写法，需要预先声明变量
            string s;
            OutSample(out s);
            lblMsg.Text += s;
            lblMsg.Text += Environment.NewLine;

            // 这是 c#7 的写法，不用预先声明变量了
            OutSample(out string ss);
            lblMsg.Text += ss;
            lblMsg.Text += Environment.NewLine;

            // 这是 c#7 的写法，不用预先声明变量了，并且可以使用 var
            OutSample(out var sss);
            lblMsg.Text += sss;
            lblMsg.Text += Environment.NewLine;
        }
        private void OutSample(out string str)
        {
            str = "xyz";

            /*
             * 注：
             * 1、对于 out 类型来说，是在方法内部初始化的，在 c#7 之前需要在方法外部声明（这显得没有必要，所以有了如今的改进）
             * 2、对于 ref 类型来说，是不可能改成 out 这种新方式的，因为 ref 是引用，其作用是方法外部初始化，方法内部改之
             */
        }


        // 数字语法改进（numeric literal syntax improvements）
        private void sample2()
        {
            int a1 = 123456;
            int a2 = 123_456; // 允许数字中出现“_”来提高可读性
            lblMsg.Text += a1.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += a2.ToString();
            lblMsg.Text += Environment.NewLine;

            int b1 = 0xABCDEF;
            int b2 = 0xAB_CD_EF; // 允许数字中出现“_”来提高可读性
            lblMsg.Text += b1.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += b2.ToString();
            lblMsg.Text += Environment.NewLine;
        }


        // 值类型的异步返回（generalized async return types）
        private async void sample3()
        {
            lblMsg.Text += (await GetNumber1()).ToString();
            lblMsg.Text += Environment.NewLine;

            lblMsg.Text += (await GetNumber2()).ToString();
            lblMsg.Text += Environment.NewLine;
        }
        // 在 c#7 之前异步返回 int 是这么写的，Task 是引用类型
        private async Task<int> GetNumber1()
        {
            await Task.Delay(100);
            return 1;
        }
        // 在 c#7 中异步返回 int 可以这么写，ValueTask 是值类型，提高了效率
        // 注：需要通过 nuget 引用 System.Threading.Tasks.Extensions
        private async ValueTask<int> GetNumber2()
        {
            await Task.Delay(100);
            return 1;
        }
    }
}
