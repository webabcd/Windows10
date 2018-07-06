/*
 * 演示如何复制 html 数据到剪切板，以及如何从剪切板中获取 html 数据 
 * 
 * HtmlFormatHelper - 在 Clipboard 中传递 html 数据或在 ShareContract 中传递 html 数据时的帮助类
 *     CreateHtmlFormat() - 封装需要传递的 html 字符串，以便以 html 方式传递数据
 *     GetStaticFragment() - 解封装传递过来的经过封装的 html 数据，从而获取初始需要传递的 html 字符串
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.App2AppCommunication
{
    public sealed partial class ClipboardCopyHtml : Page
    {
        public ClipboardCopyHtml()
        {
            this.InitializeComponent();
        }

        // 复制 html 字符串到剪切板
        private void btnCopyHtml_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            // 封装一下需要复制的 html 数据，以便以 html 的方式将数据复制到剪切板
            string htmlFormat = HtmlFormatHelper.CreateHtmlFormat("<body>I am webabcd</body>");
            dataPackage.SetHtmlFormat(htmlFormat);

            try
            {
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                lblMsg.Text = "已将内容复制到剪切板";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        // 显示剪切板中的 html 数据
        private async void btnPasteHtml_Click(object sender, RoutedEventArgs e)
        {
            DataPackageView dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.Html))
            {
                try
                {
                    // 封装后的数据
                    string htmlFormat = await dataPackageView.GetHtmlFormatAsync();
                    // 封装前的数据
                    string htmlFragment = HtmlFormatHelper.GetStaticFragment(htmlFormat);

                    lblMsg.Text = "htmlFormat(封装后的数据): ";
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += htmlFormat;
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += "htmlFragment(封装前的数据): ";
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += htmlFragment;
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
            else
            {
                lblMsg.Text = "剪切板中无 html 内容";
            }
        }
    }
}
