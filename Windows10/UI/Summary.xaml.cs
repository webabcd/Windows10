using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.UI
{
    public sealed partial class Summary : Page
    {
        public Summary()
        {
            this.InitializeComponent();

            this.Loaded += Summary_Loaded;
        }

        private void Summary_Loaded(object sender, RoutedEventArgs e)
        {
        
        }
    }
}
