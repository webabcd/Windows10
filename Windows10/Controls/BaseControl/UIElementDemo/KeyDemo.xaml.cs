/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     KeyDown - 按键按下时触发的事件
 *     KeyUp - 按键抬起时触发的事件
 * 
 * 
 * KeyRoutedEventArgs - 按键的事件参数
 *     Handled - 是否将事件标记为已处理
 *     Key - 按键枚举（比如 A, B, C, D, LeftControl 之类的，详细参见 VirtualKey 枚举）
 *     OriginalKey - 按键枚举（如果输入设备未映射到 Key 则可以通过此属性获取）
 *     KeyStatus - 按键的状态（一个 CorePhysicalKeyStatus 类型的对象，有好多字段，详细参见文档）
 *    
 * 
 * 注：全局键盘事件监听请参见：/Controls/BaseControl/DependencyObjectDemo/CoreDispatcherDemo.xaml.cs
 *    
 *    
 * 本例用于演示 UIElement 的 Key 相关事件的应用
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class KeyDemo : Page
    {
        public KeyDemo()
        {
            this.InitializeComponent();
        }

        // 通过输入法输入是不会触发此事件的
        private void textBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            lblMsg.Text = $"IsExtendedKey:{e.KeyStatus.IsExtendedKey}, IsKeyReleased:{e.KeyStatus.IsKeyReleased}, IsMenuKeyDown:{e.KeyStatus.IsMenuKeyDown}, RepeatCount:{e.KeyStatus.RepeatCount}, ScanCode:{e.KeyStatus.ScanCode}, WasKeyDown:{e.KeyStatus.WasKeyDown}";

            textBox.Text += e.Key.ToString();

            e.Handled = true;
        }

        // 通过输入法输入是不会触发此事件的
        private void textBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            lblMsg.Text = $"IsExtendedKey:{e.KeyStatus.IsExtendedKey}, IsKeyReleased:{e.KeyStatus.IsKeyReleased}, IsMenuKeyDown:{e.KeyStatus.IsMenuKeyDown}, RepeatCount:{e.KeyStatus.RepeatCount}, ScanCode:{e.KeyStatus.ScanCode}, WasKeyDown:{e.KeyStatus.WasKeyDown}";

            e.Handled = true;
        }
    }
}
