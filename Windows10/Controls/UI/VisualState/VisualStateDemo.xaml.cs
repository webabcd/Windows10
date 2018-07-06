/*
 * 演示“VisualState 和 VisualStateManager”相关知识点
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.UI.VisualState
{
    public sealed partial class VisualStateDemo : Page
    {
        public VisualStateDemo()
        {
            this.InitializeComponent();
        }

        private void btnVisualStateManager_Click(object sender, RoutedEventArgs e)
        {
            /*
             * bool GoToState(Control control, string stateName, bool useTransitions) - 转换 VisualState
             *     control - 需要转换 VisualState 的控件
             *     stateName - 目标 VisualState 的名称
             *     useTransitions - 是否使用 VisualTransition 进行过渡
             */

            // 将 VisualState 转到指定的状态（每个 VisualStateGroup 分别指定一个其内的 VisualState）
            VisualStateManager.GoToState(btnDemo, "PointerOver", true);
            VisualStateManager.GoToState(btnDemo, "MyState3", false);



            /*
             * VisualStateManager.GetVisualStateGroups(FrameworkElement obj) - 获取指定 FrameworkElement 中的 VisualStateGroup 集合
             *     注：本例中的 VisualState 定义在 btnDemo 的控件模板中的第一个 Grid 中
             *
             * VisualStateGroup - VisualState 组（每个 VisualStateManager 下可以有多个 VisualStateGroup）
             *     Name - 获取此 VisualStateGroup 的名字
             *     CurrentState - 获取此 VisualStateGroup 的当前使用的 VisualState（每个 VisualStateGroup 正在使用的 VisualState 只能有一个）
             *     States - 获取此 VisualStateGroup 中的 VisualState 集合
             *     Transitions - 获取此 VisualStateGroup 中的 VisualTransition 集合
             *     CurrentStateChanging, CurrentStateChanged - 此 VisualStateGroup 中的正在使用的 VisualState 发生变化时触发的事件
             *
             * VisualState - VisualState
             *     Name - 获取此 VisualState 的名字
             *     Setters - 获取此 VisualState 中的 Setter 集合
             *     StateTriggers - 获取此 VisualState 中的 StateTrigger 集合
             *     Storyboard - 获取此 VisualState 中的 Storyboard 对象
             */

            lblMsg.Text = "";
            Grid grid = Helper.GetVisualChild<Grid>(btnDemo);
            IList<VisualStateGroup> visualStateGroups = VisualStateManager.GetVisualStateGroups(grid);
            foreach (VisualStateGroup visualStateGroup in visualStateGroups)
            {
                lblMsg.Text += visualStateGroup.Name + " " + visualStateGroup.CurrentState.Name;
                lblMsg.Text += Environment.NewLine;
            }
        }
    }
}
