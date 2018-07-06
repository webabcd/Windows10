/*
 * 本例用于演示如何通过 Storyboard 使用关键帧动画，关键帧动画一共有 4 种类型：ColorAnimationUsingKeyFrames, DoubleAnimationUsingKeyFrames, PointAnimationUsingKeyFrames, ObjectAnimationUsingKeyFrames, 它们均继承自 Timeline
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Animation
{
    public sealed partial class KeyFrameAnimation : Page
    {
        public KeyFrameAnimation()
        {
            this.InitializeComponent();
        }
    }
}
