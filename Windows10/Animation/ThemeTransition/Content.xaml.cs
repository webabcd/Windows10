using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class Content : Page
    {
        public Content()
        {
            this.InitializeComponent();
        }

        // 改变 ContentControl 的内容
        private void contentControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle();
            Random random = new Random();

            rectangle.Height = 200;
            rectangle.Width = 200;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));

            contentControl.Content = rectangle;
        }

        // 绑定最新的数据到 ScrollViewer
        private void scrollViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Random random = new Random();
            scrollViewer.DataContext = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));
        }
    }
}
