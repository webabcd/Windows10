/*
 * CalendarView - 日历控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 * 
 * CalendarViewDayItem - 日历项控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     Date - 获取此日历项的日期（只读）
 *     IsBlackout - 此日历项是否不可用（通过 CalendarView 的 BlackoutForeground 属性可配置不可用日历项的前景色）
 */

using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.Controls.DateControl
{
    public sealed partial class CalendarViewDemo : Page
    {
        public CalendarViewDemo()
        {
            this.InitializeComponent();
        }

        private void cmbSelectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calendarView.SelectionMode = (CalendarViewSelectionMode)Enum.Parse(typeof(CalendarViewSelectionMode), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }

        private void cmbDisplayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calendarView.DisplayMode = (CalendarViewDisplayMode)Enum.Parse(typeof(CalendarViewDisplayMode), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }

        private void cmbFirstDayOfWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calendarView.FirstDayOfWeek = (Windows.Globalization.DayOfWeek)Enum.Parse(typeof(Windows.Globalization.DayOfWeek), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }


        // 加载日历项时触发的事件
        private void calendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            // 如果当前加载的日历项时当天的话
            if (args.Item.Date.Date.Equals(DateTime.Now.Date))
            {
                // 修改日历项的背景色（在 CalendarView 控件中没有属性可以直接设置当天日历项的背景色）
                // 另外，还有一些日历项的样式无法通过 CalendarView 直接设置，那么都可以考虑像这样做
                args.Item.Background = new SolidColorBrush(Colors.Orange);
            }
        }

        // 选中的日期发生变化时触发的事件
        private void calendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            // args.RemovedDates - 之前被选中的日期集合
            // args.AddedDates - 当前被选中的日期集合

            // calendarView.SelectedDates - 当前被选中的日期集合
        }
    }
}
