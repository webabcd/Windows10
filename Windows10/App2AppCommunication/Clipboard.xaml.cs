/*
 * 演示剪切板的基础知识，以及如何复制 text 数据到剪切板，以及如何从剪切板中获取 text 数据 
 * 
 * Clipboard - 剪切板
 *     SetContent() - 将指定的 DataPackage 存入剪切板
 *     GetContent() - 从剪切板中获取 DataPackage 对象
 *     Clear() - 清除剪切板中的全部数据
 *     Flush() - 正常情况下，关闭 app 后，此 app 保存到剪切板的数据就会消失；调用此方法后，即使关闭 app，剪切板中的数据也不会消失
 *     ContentChanged - 剪切板中的数据发生变化时所触发的事件
 *     
 * DataPackage - 用于封装 Clipboard 或 ShareContract 的数据（详细说明见“分享”的 Demo）
 *     SetText(), SetWebLink(), SetApplicationLink(), SetHtmlFormat(), SetBitmap(), SetStorageItems(), SetData(), SetDataProvider() - 设置复制到剪切板的各种格式的数据（注：一个 DataPackage 可以有多种不同格式的数据）
 *     RequestedOperation - 操作类型（DataPackageOperation 枚举: None, Copy, Move, Link），没发现此属性有任何作用
 *     
 * DataPackageView - DataPackage 对象的只读版本，从剪切板获取数据或者共享目标接收数据均通过此对象来获取 DataPackage 对象的数据（详细说明见“分享”的 Demo）
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.App2AppCommunication
{
    public sealed partial class Clipboard : Page
    {
        public Clipboard()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged += Clipboard_ContentChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.Clipboard.ContentChanged -= Clipboard_ContentChanged;
        }

        void Clipboard_ContentChanged(object sender, object e)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "剪切板中的内容发生了变化";
        }

        // 复制一段文本到剪切板
        private void btnCopyText_Click(object sender, RoutedEventArgs e)
        {
            // 构造保存到剪切板的 DataPackage 对象
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText("I am webabcd: " + DateTime.Now.ToString());

            try
            {
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage); // 保存 DataPackage 对象到剪切板
                Windows.ApplicationModel.DataTransfer.Clipboard.Flush(); // 当此 app 关闭后，依然保留剪切板中的数据
                lblMsg.Text = "已将内容复制到剪切板";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        // 显示剪切板中的文本数据
        private async void btnPasteText_Click(object sender, RoutedEventArgs e)
        {
            // 获取剪切板中的数据
            DataPackageView dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

            // 如果剪切板中有文本数据，则获取并显示该文本
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                try
                {
                    string text = await dataPackageView.GetTextAsync();
                    lblMsg.Text = text;
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
            else
            {
                lblMsg.Text = "剪切板中无文本内容";
            }
        }

        // 显示剪切板中包含的数据的格式类型，可能会有 StandardDataFormats 枚举的格式，也可能会有自定义的格式（关于自定义格式可以参见“分享”的 Demo）
        private void btnShowAvailableFormats_Click(object sender, RoutedEventArgs e)
        {
            DataPackageView dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (dataPackageView != null && dataPackageView.AvailableFormats.Count > 0)
            {
                var availableFormats = dataPackageView.AvailableFormats.GetEnumerator();
                while (availableFormats.MoveNext())
                {
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += availableFormats.Current;
                }
            }
            else
            {
                lblMsg.Text = "剪切板中无任何内容";
            }
        }

        // 清除剪切板中的全部数据
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.Clipboard.Clear();
        }
    }
}
