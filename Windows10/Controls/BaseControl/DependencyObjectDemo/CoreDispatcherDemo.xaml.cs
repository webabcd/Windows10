/*
 * DependencyObject - 依赖对象（可以在 DependencyObject 上定义 DependencyProperty）
 *     Dispatcher - 获取 CoreDispatcher 对象
 *     
 *     
 * CoreDispatcher - 核心调度器
 *     CurrentPriority - 获取或设置当前任务的优先级（CoreDispatcherPriority 枚举）
 *     HasThreadAccess - 获取一个值，用于说明当前线程是否可更新此 CoreDispatcher 线程上的 UI
 *     RunAsync(), RunIdleAsync(), TryRunAsync(), TryRunIdleAsync() - 在此 CoreDispatcher 线程上使用指定的优先级执行指定的方法（一般用于更新 CoreDispatcher 线程上的 UI）
 *     ShouldYield() - 用于获知当前任务队列中是否存在更高优先级的任务，或指定的优先级及其以上的任务
 *     AcceleratorKeyActivated - 有按键操作时触发的事件（针对 UIElement 的键盘事件监听请参见：/Controls/BaseControl/UIElementDemo/KeyDemo.xaml）
 *     
 * CoreDispatcherPriority - 任务优先级枚举（High, Normal, Low, Idle）
 * 
 * 在 UWP 中优先级从高到低的排序如下
 * 1、本地代码中的 SendMessage
 * 2、CoreDispatcherPriority.High
 * 3、CoreDispatcherPriority.Normal
 * 4、所有设备输入消息
 * 5、CoreDispatcherPriority.Low
 * 6、CoreDispatcherPriority.Idle（一般用于后台任务）
 * 
 * 
 * 本例用于演示 DependencyObject 的 Dispatcher（CoreDispatcher 类型） 的应用
 */

using System;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.DependencyObjectDemo
{
    public sealed partial class CoreDispatcherDemo : Page
    {
        public CoreDispatcherDemo()
        {
            this.InitializeComponent();

            this.Loaded += CoreDispatcherDemo_Loaded;

            // 监听按键事件
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private void CoreDispatcherDemo_Loaded(object sender, RoutedEventArgs e)
        {
            Timer timer =  new Timer((p) => 
            {
                // 当前线程是否可修改 CoreDispatcher 上的 UI
                if (base.Dispatcher.HasThreadAccess)
                {
                    lblMsg1.Text = "相同线程 " + DateTime.Now.ToString("mm:ss");
                }
                else
                {
                    // 非 UI 线程通过 CoreDispatcher 更新 UI
                    var ignored = base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        lblMsg1.Text = "不同线程 " + DateTime.Now.ToString("mm:ss");
                    });
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));


            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.FromSeconds(1);
            dTimer.Tick += (x, y) => 
            {
                // 当前线程是否可修改 Dispatcher 上的 UI
                if (base.Dispatcher.HasThreadAccess)
                {
                    lblMsg2.Text = "相同线程 " + DateTime.Now.ToString("mm:ss");
                }
                else
                {
                    // 非 UI 线程通过 CoreDispatcher 更新 UI
                    var ignored = base.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        lblMsg2.Text = "不同线程 " + DateTime.Now.ToString("mm:ss");
                    });
                }
            };
            dTimer.Start();
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            /*
             * AcceleratorKeyEventArgs - 按键事件参数
             *     EventType - 事件类型（比如 KeyDown, KeyUp 之类的，详细参见 CoreAcceleratorKeyEventType 枚举）
             *     VirtualKey - 按键枚举（比如 A, B, C, D, LeftControl 之类的，详细参见 VirtualKey 枚举）
             *     KeyStatus - 按键的状态（一个 CorePhysicalKeyStatus 类型的对象，有好多字段，详细参见文档）
             */

            lblMsg3.Text += $"EventType:{args.EventType}, VirtualKey:{args.VirtualKey}, IsExtendedKey:{args.KeyStatus.IsExtendedKey}, IsKeyReleased:{args.KeyStatus.IsKeyReleased}, IsMenuKeyDown:{args.KeyStatus.IsMenuKeyDown}, RepeatCount:{args.KeyStatus.RepeatCount}, ScanCode:{args.KeyStatus.ScanCode}, WasKeyDown:{args.KeyStatus.WasKeyDown}";
            lblMsg3.Text += Environment.NewLine;

            // 屏蔽系统对按键的处理
            args.Handled = true;
        }
    }
}
