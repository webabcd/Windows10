/*
 * 演示不同语言环境下对数字的格式化
 */

using System;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Localization
{
    public sealed partial class NumberFormatting : Page
    {
        public NumberFormatting()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 百分比格式化
            PercentFormatter percentFormatter = new PercentFormatter();
            // PercentFormatter percentFormatter = new PercentFormatter(new[] { "zh-Hans-CN" }, "CN");
            lblMsg.Text = percentFormatter.Format(3.1415926);
            lblMsg.Text += Environment.NewLine;

            // 千分比格式化
            PermilleFormatter permilleFormatter = new PermilleFormatter();
            //  PermilleFormatter permilleFormatter = new PermilleFormatter(new[] { "zh-Hans-CN" }, "CN");
            lblMsg.Text += permilleFormatter.Format(3.1415926);
            lblMsg.Text += Environment.NewLine;

            // 数字格式化
            DecimalFormatter decimalFormatter = new DecimalFormatter();
            // DecimalFormatter decimalFormatter = new DecimalFormatter(new[] { "zh-Hans-CN" }, "CN");
            lblMsg.Text += decimalFormatter.Format(3.1415926);
            lblMsg.Text += Environment.NewLine;

            // 货币格式化
            CurrencyFormatter currencyFormatter = new CurrencyFormatter("CNY");
            // CurrencyFormatter currencyFormatter = new CurrencyFormatter("CNY", new[] { "zh-Hans-CN" }, "CN");
            lblMsg.Text += currencyFormatter.Format(3.1415926);
        }
    }
}
