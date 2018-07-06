/*
 * 演示如何调用自定义文件保存选取器
 * 
 * 自定义文件保存选取器参见 MySavePicker.xaml
 */

using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class MySavePickerDemo : Page
    {
        public MySavePickerDemo()
        {
            this.InitializeComponent();
        }

        private async void btnMySavePicker_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.FileTypeChoices.Add("文本", new List<string>() { ".txt" });

            // 弹出文件保存窗口
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                /*
                 * 运行到此，只是在目标地址创建了一个没有任何内容的空白文件而已，接下来开始向文件写入内容
                 */

                // 告诉 Windows ，从此时开始要防止其它程序更新指定的文件
                CachedFileManager.DeferUpdates(file);

                // 将指定的内容保存到指定的文件
                string textContent = "I am webabcd";
                await FileIO.WriteTextAsync(file, textContent);

                // 告诉 Windows ，从此时开始允许其它程序更新指定的文件
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    lblMsg.Text = "文件 " + file.Name + " 保存成功";
                }

                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "FileUpdateStatus: " + status.ToString();
            }
            else
            {
                lblMsg.Text = "取消了";
            }
        }
    }
}
