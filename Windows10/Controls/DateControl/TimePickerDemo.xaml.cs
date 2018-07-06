/*
 * TimePicker - 时间选择控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     Time, MinuteIncrement, ClockIdentifier - 详见下面示例代码中的注释
 */

using System;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.DateControl
{
    public sealed partial class TimePickerDemo : Page
    {
        public TimePickerDemo()
        {
            this.InitializeComponent();

            this.Loaded += TimePickerDemo_Loaded;
        }

        void TimePickerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // Time - TimePicker 控件当前显示的时间
            timePicker1.Time = new TimeSpan(16, 0, 0);

            // MinuteIncrement - 分钟选择框的分钟增量（0, 15, 30, 45）
            timePicker1.MinuteIncrement = 15;

            // ClockIdentifier - 小时制式，ClockIdentifiers.TwelveHour（12HourClock），12 小时制
            timePicker1.ClockIdentifier = ClockIdentifiers.TwelveHour;

            // ClockIdentifier - 小时制式，ClockIdentifiers.TwentyFourHour（24HourClock），24 小时制
            timePicker2.ClockIdentifier = ClockIdentifiers.TwentyFourHour;
        }

        // TimePicker 控件选中的时间发生变化时触发的事件
        private void timePicker1_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            // e.OldTime - 原时间
            // e.NewTime - 新时间
            lblMsg.Text = $"OldTime - {e.OldTime.ToString("c")}, NewTime - {e.NewTime.ToString("c")}";
        }
    }
}