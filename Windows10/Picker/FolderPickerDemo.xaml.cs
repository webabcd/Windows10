/*
 * 演示如何通过 FolderPicker 选择一个文件夹
 * 
 * FolderPicker - 文件夹选择窗口
 *     ViewMode - 文件夹选择窗口的视图模式，Windows.Storage.Pickers.PickerViewMode 枚举（List 或 Thumbnail）
 *     SuggestedStartLocation - 文件夹选择窗口所显示的初始路径，Windows.Storage.Pickers.PickerLocationId 枚举
 *         DocumentsLibrary, ComputerFolder, Desktop,, Downloads, HomeGroup, MusicLibrary, PicturesLibrary, VideosLibrary, Objects3D, Unspecified
 *     FileTypeFilter - 允许显示在文件夹选择窗口的文件类型集合（只能显示符合要求的文件，但是无法选中）
 *     CommitButtonText - 文件夹选择窗口的提交按钮的显示文本，此按钮默认显示的文本为“选择这个文件夹”
 *     PickSingleFolderAsync() - 弹出文件夹选择窗口，以让用户选择一个文件夹
 */

using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class FolderPickerDemo : Page
    {
        public FolderPickerDemo()
        {
            this.InitializeComponent();
        }

        private async void btnPickFolder_Click(object sender, RoutedEventArgs e)
        {
            // 选择一个文件夹
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add(".docx");
            folderPicker.FileTypeFilter.Add(".xlsx");
            folderPicker.FileTypeFilter.Add(".pptx");

            // 弹出文件夹选择窗口
            StorageFolder folder = await folderPicker.PickSingleFolderAsync(); // 用户在“文件夹选择窗口”中完成操作后，会返回对应的 StorageFolder 对象
            if (folder != null)
            {
                lblMsg.Text = "选中文件夹: " + folder.Name;
            }
            else
            {
                lblMsg.Text = "取消了";
            }
        }
    }
}
