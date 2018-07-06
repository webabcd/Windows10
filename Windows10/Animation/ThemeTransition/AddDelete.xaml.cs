using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class AddDelete : Page
    {
        public AddDelete()
        {
            this.InitializeComponent();
        }

        // 添加项
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle();
            Random random = new Random();

            rectangle.Height = 100;
            rectangle.Width = 100;
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)));

            itemsControl.Items.Add(rectangle);
        }

        // 删除项
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (itemsControl.Items.Count > 0)
                itemsControl.Items.RemoveAt(itemsControl.Items.Count - 1);
        }
    }
}
