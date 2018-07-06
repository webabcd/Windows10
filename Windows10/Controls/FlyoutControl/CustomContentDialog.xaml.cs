/*
 * 自定义 ContentDialog
 */

using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class CustomContentDialog : ContentDialog
    {
        public CustomContentDialog()
        {
            this.InitializeComponent();
        }

        // 用户点击了第一个按钮
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 通过 GetDeferral() 来等待长时任务，否则即使 await 了也不会等
            ContentDialogButtonClickDeferral deferral = args.GetDeferral();
            try
            {
                await Task.Delay(1);
            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }

            // 使此事件可以冒泡（当然 args.Cancel 默认就是 false）
            args.Cancel = false;
        }

        // 用户点击了第二个按钮
        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 通过 GetDeferral() 来等待长时任务，否则即使 await 了也不会等
            ContentDialogButtonClickDeferral deferral = args.GetDeferral();
            try
            {
                await Task.Delay(1);
            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }

            // 使此事件可以冒泡（当然 args.Cancel 默认就是 false）
            args.Cancel = false;
        }
    }
}
