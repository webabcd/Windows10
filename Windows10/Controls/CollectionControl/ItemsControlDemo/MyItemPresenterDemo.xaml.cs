/*
 * 本例用于演示如何自定义 ContentPresenter 实现类似 GridViewItemPresenter 和 ListViewItemPresenter 的效果
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class MyItemPresenterDemo : Page
    {
        public MyItemPresenterDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            gridView.ItemsSource = TestData.GetEmployees();
        }
    }
}
