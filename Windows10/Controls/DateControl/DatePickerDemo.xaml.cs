/*
 * DatePicker - 日期选择控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     YearVisible, MonthVisible, DayVisible, MinYear, MaxYear, Date, CalendarIdentifier - 详见下面示例代码中的注释
 */

using System;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.DateControl
{
    public sealed partial class DatePickerDemo : Page
    {
        public DatePickerDemo()
        {
            this.InitializeComponent();

            this.Loaded += DatePickerDemo_Loaded;
        }

        void DatePickerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // Date - DatePicker 控件当前显示的日期
            datePicker.Date = DateTimeOffset.Now.AddMonths(1);

            // MinYear - DatePicker 控件所允许选择的最小的年份
            datePicker.MinYear = DateTimeOffset.Now.AddYears(-20);
            // MaxYear - DatePicker 控件所允许选择的最大的年份
            datePicker.MaxYear = DateTimeOffset.Now.AddYears(20);

            // YearVisible -  是否显示 year 选择框
            datePicker.YearVisible = true;
            // MonthVisible -  是否显示 month 选择框
            datePicker.MonthVisible = true;
            // DayVisible -  是否显示 day 选择框
            datePicker.DayVisible = true;

            // CalendarIdentifier - DatePicker 控件使用的日历系统，一个字符串值，在 CalendarIdentifiers 类中有封装
            datePicker.CalendarIdentifier = CalendarIdentifiers.Gregorian;
        }

        // DatePicker 控件选中的日期发生变化时触发的事件
        private void datePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            // e.OldDate - 原日期
            // e.NewDate - 新日期
            lblMsg.Text = $"OldDate - {e.OldDate.ToString("yyyy-MM-dd hh:mm:ss")}, NewDate - {e.NewDate.ToString("yyyy-MM-dd hh:mm:ss")}";
        }
    }
}