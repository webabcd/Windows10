/*
 * GridViewItemPresenter, ListViewItemPresenter - 用于呈现 GridView 或 ListView 的 Item（继承自 ContentPresenter, 请参见 /Controls/BaseControl/ContentControlDemo/ContentPresenterDemo.xaml）
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class ItemPresenterDemo : Page
    {
        public ItemPresenterDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            gridView.ItemsSource = TestData.GetEmployees();
        }
    }
}
