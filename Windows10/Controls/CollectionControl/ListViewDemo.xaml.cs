/*
 * ListView - ListView 控件（继承自 ListViewBase, 请参见 /Controls/CollectionControl/ListViewBaseDemo/）
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl
{
    public sealed partial class ListViewDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>(TestData.GetEmployees());

        public ListViewDemo()
        {
            this.InitializeComponent();
        }
    }
}
