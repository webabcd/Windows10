/*
 * ButtonBase(基类) - 按钮控件基类（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.ButtonControl
{
    public sealed partial class ButtonBaseDemo : Page
    {
        public ButtonBaseDemo()
        {
            this.InitializeComponent();

            this.Loaded += ButtonBaseDemo_Loaded;
        }

        private void ButtonBaseDemo_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.Zero;
            dTimer.Tick += DTimer_Tick;
            dTimer.Start();
        }

        private void DTimer_Tick(object sender, object e)
        {
            lblMsg1.Text = $"button1 IsPointerOver:{button1.IsPointerOver}, IsPressed:{button1.IsPressed}";
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += $"button2 IsPointerOver:{button2.IsPointerOver}, IsPressed:{button2.IsPressed}";
            lblMsg1.Text += Environment.NewLine;
            // 鼠标移动到 button3 上时，其 IsPointerOver 和 IsPressed 均为 true，因为其 ClickMode 为 Hover
            lblMsg1.Text += $"button3 IsPointerOver:{button3.IsPointerOver}, IsPressed:{button3.IsPressed}";
        }

        // ClickMode.Release - 鼠标按下并抬起即触发 Click 事件（默认值）
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            lblMsg2.Text += "button1 ClickMode.Release";
            lblMsg2.Text += Environment.NewLine;
        }

        // ClickMode.Press - 鼠标按下即触发 Click 事件
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            lblMsg2.Text += "button2 ClickMode.Press";
            lblMsg2.Text += Environment.NewLine;
        }

        // ClickMode.Hover - 鼠标经过即触发 Click 事件
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            lblMsg2.Text += "button3 ClickMode.Hover";
            lblMsg2.Text += Environment.NewLine;
        }
    }
}
