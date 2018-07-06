/*
 * GridView - GridView 控件（继承自 ListViewBase, 请参见 /Controls/CollectionControl/ListViewBaseDemo/）
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl
{
    public sealed partial class GridViewDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>(TestData.GetEmployees());

        public GridViewDemo()
        {            
            this.InitializeComponent();
        }
    }
}
