using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class Reorder : Page
    {
        public Reorder()
        {
            this.InitializeComponent();
        }

        // 在集合的位置 2 处添加新的元素，以达到重新排序的效果
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle();
            Random random = new Random();

            rectangle.Height = 100;
            rectangle.Width = 100;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));

            itemsControl.Items.Insert(2, rectangle);
        }
    }
}
