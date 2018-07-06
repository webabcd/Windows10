/*
 * MessageDialog - 信息对话框（未继承任何类）
 *     Content - 内容
 *     Title - 标题
 *     Options - 选项（Windows.UI.Popups.MessageDialogOptions 枚举）
 *         None - 正常，默认值
 *         AcceptUserInputAfterDelay - 为避免用户误操作，弹出对话框后短时间内禁止单击命令按钮
 *     Commands - 对话框的命令栏中的命令集合，返回 IList<IUICommand> 类型的数据
 *     DefaultCommandIndex - 按“enter”键后，激发此索引位置的命令
 *     CancelCommandIndex - 按“esc”键后，激发此索引位置的命令
 *     ShowAsync() - 显示对话框，并返回用户激发的命令
 *     
 * IUICommand - 命令
 *     Label - 显示的文字
 *     Id - 参数
 */

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class MessageDialogDemo : Page
    {
        public MessageDialogDemo()
        {
            this.InitializeComponent();
        }

        // 弹出一个简单的 MessageDialog
        private async void btnShowMessageDialogSimple_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("内容");

            await messageDialog.ShowAsync();
        }

        // 弹出一个自定义命令按钮的 MessageDialog
        private async void btnShowMessageDialogCustomCommand_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog messageDialog = new MessageDialog("内容", "标题");

            messageDialog.Commands.Add
            (
                new UICommand
                (
                    "自定义命令按钮1",
                    (command) =>
                    {
                        lblMsg.Text = string.Format("command label:{0}, id:{1}", command.Label, command.Id);
                    },
                    "param1"
                )
            );

            messageDialog.Commands.Add
            (
                new UICommand
                (
                    "自定义命令按钮2", 
                    (command) =>
                    {
                        lblMsg.Text = string.Format("command label:{0}, id:{1}", command.Label, command.Id);
                    }, 
                    "param2"
                )
            );

            messageDialog.Commands.Add
            (
                new UICommand
                (
                    "自定义命令按钮3", 
                    (command) =>
                    {
                        lblMsg.Text = string.Format("command label:{0}, id:{1}", command.Label, command.Id);
                    }, 
                    "param3"
                )
            );

            messageDialog.DefaultCommandIndex = 0; // 按“enter”键后，激发第 1 个命令
            messageDialog.CancelCommandIndex = 2; // 按“esc”键后，激发第 3 个命令
            messageDialog.Options = MessageDialogOptions.AcceptUserInputAfterDelay; // 对话框弹出后，短时间内禁止用户单击命令按钮，以防止用户的误操作

            // 显示对话框，并返回用户激发的命令
            IUICommand chosenCommand = await messageDialog.ShowAsync();

            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += string.Format("result label:{0}, id:{1}", chosenCommand.Label, chosenCommand.Id);
        }
    }
}
