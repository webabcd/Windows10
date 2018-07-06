/*
 * 演示如何通过 FileOpenPicker 选择一个文件或多个文件
 * 
 * FileOpenPicker - 文件选择窗口
 *     ViewMode - 文件选择窗口的视图模式，Windows.Storage.Pickers.PickerViewMode 枚举（List 或 Thumbnail）
 *     SuggestedStartLocation - 文件选择窗口所显示的初始路径，Windows.Storage.Pickers.PickerLocationId 枚举
 *         DocumentsLibrary, ComputerFolder, Desktop,, Downloads, HomeGroup, MusicLibrary, PicturesLibrary, VideosLibrary, Objects3D, Unspecified
 *     FileTypeFilter - 允许显示在文件选择窗口的文件类型集合（* 代表全部）
 *     CommitButtonText - 文件选择窗口的提交按钮的显示文本，此按钮默认显示的文本为“打开”
 *     PickSingleFileAsync() -  弹出文件选择窗口，以让用户选择一个文件
 *     PickMultipleFilesAsync() - 弹出文件选择窗口，以让用户选择多个文件
 */

using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class FileOpenPickerDemo : Page
    {
        public FileOpenPickerDemo()
        {
            this.InitializeComponent();
        }

        private async void btnPickFile_Click(object sender, RoutedEventArgs e)
        {
            // 选择一个文件
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.CommitButtonText = "选中此文件";
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".png");
            // * 代表全部
            // openPicker.FileTypeFilter.Add("*");

            // 弹出文件选择窗口
            StorageFile file = await openPicker.PickSingleFileAsync(); // 用户在“文件选择窗口”中完成操作后，会返回对应的 StorageFile 对象
            if (file != null)
            {
                lblMsg.Text = "选中文件: " + file.Name;
            }
            else
            {
                lblMsg.Text = "取消了";
            }
        }

        private async void btnPickFiles_Click(object sender, RoutedEventArgs e)
        {
            // 选择多个文件
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add("*");

            // 弹出文件选择窗口
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync(); // 用户在“文件选择窗口”中完成操作后，会返回对应的 StorageFile 对象
            if (files.Count > 0)
            {
                lblMsg.Text = "选中文件: ";
                lblMsg.Text += Environment.NewLine;
                foreach (StorageFile file in files)
                {
                    lblMsg.Text += (file.Name);
                    lblMsg.Text += Environment.NewLine;
                }
            }
            else
            {
                lblMsg.Text = "取消了";
            }
        }
    }
}
