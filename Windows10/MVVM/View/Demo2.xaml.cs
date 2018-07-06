using Windows.UI.Xaml.Controls;
using Windows10.MVVM.ViewModel2;

namespace Windows10.MVVM.View
{
    public sealed partial class Demo2 : Page
    {
        public ProductViewModel ProductViewModel { get; set; } = new ProductViewModel();

        public Demo2()
        {
            this.InitializeComponent();
        }
    }
}
