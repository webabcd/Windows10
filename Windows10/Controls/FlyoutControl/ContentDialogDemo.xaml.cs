/*
 * ContentDialog - 内容对话框（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 *     FullSizeDesired - 是否全屏弹出对话框
 *     Title, TitleTemplate - 对话框的标题（可以自定义样式）
 *     Content, ContentTemplate - 对话框的内容（可以自定义样式）
 *     PrimaryButtonText - 对话框第一个按钮显示的文本
 *     SecondaryButtonText - 对话框第二个按钮显示的文本
 *     PrimaryButtonCommand, PrimaryButtonCommandParameter, SecondaryButtonCommand, SecondaryButtonCommandParameter - 按钮命令及命令参数
 *     
 *     PrimaryButtonClick - 第一个按钮按下时触发的事件
 *     SecondaryButtonClick - 第二个按钮按下时触发的事件
 *     Closed, Closing, Opened - 顾名思义的一些事件
 *     
 *     ShowAsync() - 弹出对话框
 *     Hide() - 隐藏对话框
 *     
 * 
 * 注意：自定义的内容对话框参见 CustomContentDialog.xaml
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class ContentDialogDemo : Page
    {
        public ContentDialogDemo()
        {
            this.InitializeComponent();
        }

        private async void btnShowDialog_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "dialog title",
                Content = "dialog content, dialog content, dialog content, dialog content, dialog content, dialog content, dialog content",
                FullSizeDesired = true,
                PrimaryButtonText = "PrimaryButton",
                SecondaryButtonText = "SecondaryButton"
            };

            dialog.PrimaryButtonClick += dialog_PrimaryButtonClick;
            dialog.SecondaryButtonClick += dialog_SecondaryButtonClick;

            // 弹出对话框，返回值为用户的选择结果
            /*
             * ContentDialogResult.Primary - 用户选择了第一个按钮
             * ContentDialogResult.Secondary - 用户选择了第二个按钮
             * ContentDialogResult.None - 用户没有选择（按了系统的“返回”按钮）
             */
            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                lblMsg.Text += "选择了第一个按钮";
                lblMsg.Text += Environment.NewLine;
            }
            else if (result == ContentDialogResult.Secondary)
            {
                lblMsg.Text += "选择了第二个按钮";
                lblMsg.Text += Environment.NewLine;
            }
            else if (result == ContentDialogResult.None)
            {
                lblMsg.Text += "没有选择按钮";
                lblMsg.Text += Environment.NewLine;
            }
        }

        void dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            lblMsg.Text += "点击了第一个按钮";
            lblMsg.Text += Environment.NewLine;
        }

        void dialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            lblMsg.Text += "点击了第二个按钮";
            lblMsg.Text += Environment.NewLine;
        }


        // 弹出自定义对话框
        async private void btnShowCustomDialog_Click(object sender, RoutedEventArgs e)
        {
            // 实例化自定义对话框
            CustomContentDialog contentDialog = new CustomContentDialog();

            // 弹出对话框，返回值为用户的选择结果
            /*
             * ContentDialogResult.Primary - 用户选择了第一个按钮
             * ContentDialogResult.Secondary - 用户选择了第二个按钮
             * ContentDialogResult.None - 用户没有选择（按了系统的“返回”按钮）
             */
            ContentDialogResult result = await contentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                lblMsg.Text += "选择了第一个按钮";
                lblMsg.Text += Environment.NewLine;
            }
            else if (result == ContentDialogResult.Secondary)
            {
                lblMsg.Text += "选择了第二个按钮";
                lblMsg.Text += Environment.NewLine;
            }
            else if (result == ContentDialogResult.None)
            {
                lblMsg.Text += "没有选择按钮";
                lblMsg.Text += Environment.NewLine;
            }
        }
    }
}
