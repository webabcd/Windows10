/*
 * InkCanvas - 涂鸦板控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     InkPresenter - 获取 InkPresenter 对象
 *        
 * InkPresenter - 涂鸦板
 *     StrokeContainer - 返回 InkStrokeContainer 类型的对象
 *     
 * InkStrokeContainer - 用于管理涂鸦
 *     IAsyncOperationWithProgress<UInt32, UInt32> SaveAsync(IOutputStream outputStream) - 保存涂鸦数据
 *     IAsyncActionWithProgress<UInt64> LoadAsync(IInputStream inputStream) - 加载涂鸦数据
 */

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class InkCanvasDemo3 : Page
    {
        public InkCanvasDemo3()
        {
            this.InitializeComponent();

            inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;

            InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.IgnorePressure = true;
            drawingAttributes.Color = Colors.Red;
            drawingAttributes.Size = new Size(4, 4);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Count == 0)
                return;

            // 用于保存涂鸦数据
            IRandomAccessStream stream = new InMemoryRandomAccessStream();
            await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);

            // 文件保存对话框
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeChoices.Add("ink files", new List<string>() { ".ink" });

            // 弹出文件保存对话框
            var file = await picker.PickSaveFileAsync();
            if (file == null)
                return;

            // 在调用 CompleteUpdatesAsync 之前，阻止对文件的更新
            CachedFileManager.DeferUpdates(file);

            // Stream 转 byte[]
            DataReader reader = new DataReader(stream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)stream.Size);
            byte[] bytes = new byte[stream.Size];
            reader.ReadBytes(bytes);

            // 写入文件
            await FileIO.WriteBytesAsync(file, bytes);

            // 保存文件
            await CachedFileManager.CompleteUpdatesAsync(file);
        }

        private async void load_Click(object sender, RoutedEventArgs e)
        {
            // 文件打开对话框
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".ink");

            // 弹出文件打开对话框
            var pickedFile = await picker.PickSingleFileAsync();
            if (pickedFile != null)
            {
                // 读取涂鸦数据
                IRandomAccessStreamWithContentType stream = await pickedFile.OpenReadAsync();

                // 加载指定的涂鸦数据
                await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
            }
        }
    }
}
