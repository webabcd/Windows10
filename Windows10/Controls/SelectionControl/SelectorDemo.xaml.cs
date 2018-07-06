/*
 * Selector(基类) - 选择器控件基类（继承自 ItemsControl, 请参见 /Controls/CollectionControl/ItemsControlDemo/）
 *     SelectedIndex - 选中项的索引
 *     SelectedItem - 选中项的数据对象
 *     SelectedValuePath - 选中项的值的字段路径，默认值为空字符串（此时 SelectedValue 的结果与 SelectedItem 相同）
 *     SelectedValue - 选中项的值（字段路径通过 SelectedValuePath 设置）
 *     bool GetIsSelectionActive(DependencyObject element) - 用于获取指定的 Selector 控件是否是焦点状态
 *         如果是焦点状态，则按下键盘 enter 键会弹出此 Selector 控件的选项列表，按下 esc 键会隐藏此 Selector 控件的选项列表
 *     IsSynchronizedWithCurrentItem - 暂时认为没用吧，因为设置为 true 后，在 runtime 会报错
 *     SelectionChanged - 选中项发生变化时触发的事件
 *     
 *     
 * SelectorItem(基类) - Selector 的 Item（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 *     IsSelected - 是否被选中
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.SelectionControl
{
    public sealed partial class SelectorDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(30);

        public SelectorDemo()
        {
            this.InitializeComponent();

            this.Loaded += SelectorDemo_Loaded;

            // 不设置 SelectedValuePath，则 SelectedValue 的结果与 SelectedItem 相同
            comboBox1.SelectedValuePath = "";
            comboBox1.SelectionChanged += ComboBox1_SelectionChanged;

            // 指定 SelectedValue 的字段路径
            comboBox2.SelectedValuePath = "Name";
            comboBox2.SelectionChanged += ComboBox2_SelectionChanged;
        }

        private void SelectorDemo_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Interval = TimeSpan.Zero;
            dTimer.Tick += DTimer_Tick;
            dTimer.Start();
        }

        private void DTimer_Tick(object sender, object e)
        {
            textBlock.Text = $"comboBox1 focus:{ComboBox.GetIsSelectionActive(comboBox1)}, comboBox2 focus:{ComboBox.GetIsSelectionActive(comboBox2)}";
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // e.RemovedItems - 本次事件中，被取消选中的项
            // e.AddedItems - 本次事件中，新被选中的项

            int selectedIndex = comboBox1.SelectedIndex;

            // SelectedItem 是选中的 Employee 对象
            // SelectedValue 是选中的 Employee 对象
            lblMsg1.Text = $"comboBox1 SelectedItem:{comboBox1.SelectedItem}, SelectedValue:{comboBox1.SelectedValue}";
        }

        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = comboBox2.SelectedIndex;

            // SelectedItem 是选中的 Employee 对象
            // SelectedValue 是选中的 Employee 对象的 Name 属性的值
            lblMsg2.Text = $"comboBox2 SelectedItem:{comboBox2.SelectedItem}, SelectedValue:{comboBox2.SelectedValue}";
        }
    }
}