/*
 * 演示如何通过 Binding 绑定字典表
 */

using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Windows10.Bind
{
    public sealed partial class BindingDictionary : Page
    {
        private Dictionary<string, string> _data;

        public BindingDictionary()
        {
            this.InitializeComponent();

            _data = new Dictionary<string, string>();
            _data.Add("key1", "value1");
            _data.Add("key2", "value2");
            _data.Add("key3", "value3");

            combo.ItemsSource = _data;
        }
        
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KeyValuePair<string, string> selectedItem = (KeyValuePair<string, string>)combo.SelectedItem;
            lblMsg.Text = $"selectedItem: {selectedItem.Key}, {selectedItem.Value}";
        }
    }
}
