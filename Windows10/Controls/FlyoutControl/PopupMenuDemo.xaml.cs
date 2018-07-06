/*
 * PopupMenu - 上下文菜单（未继承任何类）
 *     Commands - 上下文菜单中的命令集合，返回 IList<IUICommand> 类型的数据
 *     IAsyncOperation<IUICommand> ShowAsync(Point invocationPoint) - 在指定的位置（PopupMenu 的默认显示位置在窗口 0,0 点）上显示上下文菜单，并返回用户激发的命令
 *     IAsyncOperation<IUICommand> ShowForSelectionAsync(Rect selection, Placement preferredPlacement) - 在指定的矩形区域的指定方位显示上下文菜单，并返回用户激发的命令
 *     
 * IUICommand - 命令
 *     Label - 显示的文字
 *     Id - 参数
 *
 * UICommandSeparator - 分隔符
 */

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows10.Common;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class PopupMenuDemo : Page
    {
        public PopupMenuDemo()
        {
            this.InitializeComponent();

            lblDemo.RightTapped += lblDemo_RightTapped;
        }

        private async void lblDemo_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            PopupMenu menu = new PopupMenu();

            menu.Commands.Add
            (
                new UICommand
                (
                    "item1", 
                    (command) =>
                    {
                        lblMsg.Text = string.Format("command label:{0}, id:{1}", command.Label, command.Id);
                    }, 
                    "param1"
                )
            );

            menu.Commands.Add
            (
                new UICommand
                (
                    "item2", 
                    (command) =>
                    {
                        lblMsg.Text = string.Format("command label:{0}, id:{1}", command.Label, command.Id);
                    }, 
                    "param2"
                )
            );

            // 分隔符
            menu.Commands.Add(new UICommandSeparator());

            menu.Commands.Add
            (
                new UICommand
                (
                    "item3",
                    (command) =>
                    {
                        lblMsg.Text = string.Format("command label:{0}, id:{1}", command.Label, command.Id);
                    }, 
                    "param3"
                )
            );


            // 在指定的位置显示上下文菜单，并返回用户激发的命令（测试的时候这里有时会发生异常，不知道什么原因，所以还是尽量用 MenuFlyout 吧）
            IUICommand chosenCommand = await menu.ShowForSelectionAsync(Helper.GetElementRect((FrameworkElement)sender), Placement.Below);
            if (chosenCommand == null) // 用户没有在上下文菜单中激发任何命令
            {
                lblMsg.Text = "用户没有选择任何命令";
            }
            else
            {
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += string.Format("result label:{0}, id:{1}", chosenCommand.Label, chosenCommand.Id);
            }
        }
    }
}
