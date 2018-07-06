/*
 * 本例用于演示如何使用 NavigationThemeTransition 过渡效果
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Animation.NavigationTransitionInfo
{
    public sealed partial class MyFrame : Page
    {
        public MyFrame()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*
             * 根据导航进来的参数来决定
             * <Page.Transitions>
             *     <TransitionCollection>
             *         <NavigationThemeTransition x:Name="navigationTransition" />
             *     </TransitionCollection>
             * </Page.Transitions>
             * 使用何种 NavigationTransitionInfo（即如何设置 NavigationThemeTransition 的 DefaultNavigationTransitionInfo 属性）
             */
            Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transition = e.Parameter as Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo;
            if (transition != null)
                navigationTransition.DefaultNavigationTransitionInfo = transition;
        }
    }
}
