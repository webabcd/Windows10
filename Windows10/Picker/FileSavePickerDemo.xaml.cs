/*
 * 演示如何通过 FileSavePicker 保存文件到指定路径
 * 
 * FileSavePicker - 文件保存窗口
 *     SuggestedStartLocation - 文件保存窗口所显示的初始路径，Windows.Storage.Pickers.PickerLocationId 枚举
 *         DocumentsLibrary, ComputerFolder, Desktop,, Downloads, HomeGroup, MusicLibrary, PicturesLibrary, VideosLibrary, Objects3D, Unspecified
 *     SuggestedFileName - 需要保存的文件的默认文件名
 *     SuggestedSaveFile - 需要保存的文件的默认 StorageFile 对象
 *     FileTypeChoices - 可保存的扩展名集合（* 代表全部）
 *     DefaultFileExtension - 默认扩展名
 *     CommitButtonText - 文件保存窗口的提交按钮的显示文本，此按钮默认显示的文本为“保存”
 *     PickSaveFileAsync() - 弹出文件保存窗口，以让用户保存文件
 */

using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class FileSavePickerDemo : Page
    {
        public FileSavePickerDemo()
        {
            this.InitializeComponent();
        }

        private async void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // 在扩展名选择框中将会显示：文本(.txt)
            savePicker.FileTypeChoices.Add("文本", new List<string>() { ".txt" });
            savePicker.SuggestedFileName = "webabcdFileSavePicker";

            // 弹出文件保存窗口
            StorageFile file = await savePicker.PickSaveFileAsync(); // 用户在“文件保存窗口”中完成操作后，会返回对应的 StorageFile 对象
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
