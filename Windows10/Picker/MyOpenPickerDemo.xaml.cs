/*
 * 演示如何调用自定义文件打开选取器
 * 
 * 自定义文件打开选取器参见 MyOpenPicker.xaml
 */

using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class MyOpenPickerDemo : Page
    {
        public MyOpenPickerDemo()
        {
            this.InitializeComponent();
        }

        private async void btnMyOpenPicker_Click(object sender, RoutedEventArgs e)
        {
            // 选择一个文件
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.CommitButtonText = "选中此文件";
            openPicker.FileTypeFilter.Add("*");

            // 弹出文件选择窗口
            StorageFile file = await openPicker.PickSingleFileAsync(); 
            if (file != null)
            {
                lblMsg.Text = "选中文件: " + file.Name;
            }
            else
            {
                lblMsg.Text = "取消了";
            }
        }
    }
}
