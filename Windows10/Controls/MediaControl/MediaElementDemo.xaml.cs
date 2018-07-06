/*
 * MediaElement - 视频控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     用于视频播放，懒得写了，参见文档吧
 *     也可以看看之前 win8 时写的示例 http://www.cnblogs.com/webabcd/archive/2013/01/24/2874156.html, http://www.cnblogs.com/webabcd/archive/2014/06/12/3783712.html
 *     
 * 重点需要说明的如下：
 * 1、终于直接支持 hls 协议了，即可以直接播放 m3u8
 * 2、别忘了 MediaStreamSource 
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class MediaElementDemo : Page
    {
        public MediaElementDemo()
        {
            this.InitializeComponent();
        }
    }
}
