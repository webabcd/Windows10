using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;

            Current = this;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(Index));
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoBack)
                frame.GoBack();

            SubTitle = "";
        }

        public string SubTitle
        {
            set
            {
                subTitle.Text = value;
            }
        }

        public Frame Container
        {
            get
            {
                return frame;
            }
        }
    }
}
