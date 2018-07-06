/*
 * C# 6 示例 2
 * 在 catch 和 finally 中支持 await, 异常过滤器
 */

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.CSharp6
{
    public sealed partial class Demo2 : Page
    {
        public Demo2()
        {
            this.InitializeComponent();

            this.Loaded += Demo2_Loaded;
        }

        private void Demo2_Loaded(object sender, RoutedEventArgs e)
        {
            sample1();
            sample2();
        }


        // 在 catch 和 finally 中也支持 await 了
        private async void sample1()
        {
            try
            {
                throw new Exception("");
            }
            catch
            {
                await Task.Delay(1000);
            }
            finally
            {
                await Task.Delay(1000);
            }
        }


        // 异常过滤器 (Exception filters)　
        private void sample2()
        {
            try
            {
                throw new Exception(new Random().Next(3).ToString());
            }
            catch (Exception ex) when (ex.Message.Equals("0")) // 通过 when 表达式过滤异常
            {
                lblMsg.Text += "0";
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex) when (ex.Message.Equals("1")) // 通过 when 表达式过滤异常
            {
                lblMsg.Text += "1";
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex) when (CheckExceptionMessage(ex, "2"))  // 通过 when 表达式过滤异常（表达式中的判断条件也可以是一个方法调用）
            {
                lblMsg.Text += "2";
                lblMsg.Text += Environment.NewLine;
            }
        }
        private bool CheckExceptionMessage(Exception ex, string value)
        {
            if (ex.Message.Equals(value))
                return true;
            return false;
        }
    }
}