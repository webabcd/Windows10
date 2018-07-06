/*
 * 本例用于演示如何使用 NavigationThemeTransition 过渡效果
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Windows10.Animation.NavigationTransitionInfo
{
    public sealed partial class Demo : Page
    {
        public Demo()
        {
            this.InitializeComponent();
        }



        // 以下演示如何在通过 bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride) 导航时，指定 NavigationTransitionInfo 动画效果
        // 通过 GoBack(NavigationTransitionInfo transitionInfoOverride) 导航时指定 NavigationTransitionInfo 动画效果也是同理
        // 导航目标页先要为 Page 指定 NavigationThemeTransition 过渡效果，然后导航目标页会根据导航时的 NavigationTransitionInfo 类型的参数来设置其 NavigationThemeTransition 的 DefaultNavigationTransitionInfo 属性
        private void btnGotoSlideNavigationTransitionInfo_Click(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo slideTransition = new SlideNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), null, slideTransition);
        }

        private void btnGotoSuppressNavigationTransitionInfo_Click(object sender, RoutedEventArgs e)
        {
            SuppressNavigationTransitionInfo supressTransition = new SuppressNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), null, supressTransition);
        }

        private void btnGotoDrillInNavigationTransitionInfo_Click(object sender, RoutedEventArgs e)
        {
            DrillInNavigationTransitionInfo drillInTransition = new DrillInNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), null, drillInTransition);
        }

        private void btnGotoContinuumNavigationTransitionInfo_Click(object sender, RoutedEventArgs e)
        {
            ContinuumNavigationTransitionInfo continuumTransition = new ContinuumNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), null, continuumTransition);
        }

        private void btnGotoEntranceNavigationTransitionInfo_Click(object sender, RoutedEventArgs e)
        {
            EntranceNavigationTransitionInfo entranceTransition = new EntranceNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), null, entranceTransition);
        }

        private void btnGotoCommonNavigationTransitionInfo_Click(object sender, RoutedEventArgs e)
        {
            CommonNavigationTransitionInfo commonTransition = new CommonNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), null, commonTransition);
        }



        // 以下演示如何在导航目标页设置 NavigationTransitionInfo 动画效果
        private void btnGotoSlideNavigationTransitionInfo_Click2(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo slideTransition = new SlideNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), slideTransition);
        }

        private void btnGotoSuppressNavigationTransitionInfo_Click2(object sender, RoutedEventArgs e)
        {
            SuppressNavigationTransitionInfo supressTransition = new SuppressNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), supressTransition);
        }

        private void btnGotoDrillInNavigationTransitionInfo_Click2(object sender, RoutedEventArgs e)
        {
            DrillInNavigationTransitionInfo drillInTransition = new DrillInNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), drillInTransition);
        }

        private void btnGotoContinuumNavigationTransitionInfo_Click2(object sender, RoutedEventArgs e)
        {
            ContinuumNavigationTransitionInfo continuumTransition = new ContinuumNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), continuumTransition);
        }

        private void btnGotoEntranceNavigationTransitionInfo_Click2(object sender, RoutedEventArgs e)
        {
            EntranceNavigationTransitionInfo entranceTransition = new EntranceNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), entranceTransition);
        }

        private void btnGotoCommonNavigationTransitionInfo_Click2(object sender, RoutedEventArgs e)
        {
            CommonNavigationTransitionInfo commonTransition = new CommonNavigationTransitionInfo();
            this.Frame.Navigate(typeof(MyFrame), commonTransition);
        }
    }
}
