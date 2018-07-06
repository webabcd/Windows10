/*
 * 演示全球化的基本应用
 * 
 * 
 * 注：本地化和全球化的区别
 * 1、全球化的产品应该适用于任何一个本地市场
 * 2、本地化通常会有 UI 的调整，语言的翻译，甚至是针对本地开发的一些特殊的功能
 * 3、一个全球化的产品做本地化时，一般只做语言翻译
 */

using System;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Localization
{
    public sealed partial class GlobalizationDemo : Page
    {
        public GlobalizationDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 首选语言
            lblMsg.Text = "Current Languages: " + string.Join(", ", GlobalizationPreferences.Languages);
            lblMsg.Text += Environment.NewLine;
            // 首选日历（比如：GregorianCalendar 提供了世界上大多数国家/地区使用的标准日历系统）
            lblMsg.Text += "Current Calendars: " + string.Join(", ", GlobalizationPreferences.Calendars); 
            lblMsg.Text += Environment.NewLine;
            // 时钟显示（比如：24HourClock）
            lblMsg.Text += "Current Clocks: " + string.Join(", ", GlobalizationPreferences.Clocks);
            lblMsg.Text += Environment.NewLine;
            // 区域（比如：CN）
            lblMsg.Text += "Current HomeGeographicRegion: " + GlobalizationPreferences.HomeGeographicRegion;
            lblMsg.Text += Environment.NewLine;
            // 一周的第一天是周几（比如：中国是 Monday）
            lblMsg.Text += "Current WeekStartsOn: " + GlobalizationPreferences.WeekStartsOn.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += Environment.NewLine;


            // Language - 语言对象，通过指定 BCP-47 语言标记来实例化语言对象
            Windows.Globalization.Language language = new Windows.Globalization.Language("zh-Hans-CN");
            // 判断指定的 BCP-47 语言标记的格式是否正确
            lblMsg.Text += "zh-Hans-CN IsWellFormed: " + Windows.Globalization.Language.IsWellFormed("zh-Hans-CN");
            lblMsg.Text += Environment.NewLine;
            // 语言的本地化名称
            lblMsg.Text += "zh-Hans-CN Language DisplayName: " + language.DisplayName;
            lblMsg.Text += Environment.NewLine;
            // 语言本身的名称
            lblMsg.Text += "zh-Hans-CN Language NativeName: " + language.NativeName;
            lblMsg.Text += Environment.NewLine;
            // 语言的 BCP-47 语言标记
            lblMsg.Text += "zh-Hans-CN Language LanguageTag: " + language.LanguageTag;
            lblMsg.Text += Environment.NewLine;
            // 语言的 ISO 15924 脚本代码
            lblMsg.Text += "zh-Hans-CN Language Script: " + language.Script;
            lblMsg.Text += Environment.NewLine;
            // 获取当前输入法编辑器 (IME) 的 BCP-47 语言标记
            lblMsg.Text += "zh-Hans-CN Language CurrentInputMethodLanguageTag: " + Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += Environment.NewLine;


            // GeographicRegion - 区域对象（关于 ISO 3166-1 请参见：http://zh.wikipedia.org/zh-cn/ISO_3166-1）
            GeographicRegion geographicRegion = new GeographicRegion(); // 获取当前的区域对象。
            // 区域的本地化名称
            lblMsg.Text += "Current Region DisplayName: " + geographicRegion.DisplayName;
            lblMsg.Text += Environment.NewLine;
            // 区域本身的名称
            lblMsg.Text += "Current Region NativeName: " + geographicRegion.NativeName;
            lblMsg.Text += Environment.NewLine;
            // 该区域内使用的货币类型
            lblMsg.Text += "Current Region CurrenciesInUse: " + string.Join(",", geographicRegion.CurrenciesInUse);
            lblMsg.Text += Environment.NewLine;
            // 该区域的 ISO 3166-1 二位字母标识
            lblMsg.Text += "Current Region CodeTwoLetter: " + geographicRegion.CodeTwoLetter;
            lblMsg.Text += Environment.NewLine;
            // 该区域的 ISO 3166-1 三位字母标识
            lblMsg.Text += "Current Region CodeThreeLetter: " + geographicRegion.CodeThreeLetter;
            // 该区域的 ISO 3166-1 数字标识
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Current Region CodeThreeDigit: " + geographicRegion.CodeThreeDigit;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += Environment.NewLine;


            // Calendar - 日历对象，默认返回当前系统的默认日历
            Calendar calendarDefault = new Calendar();
            // 第一个参数：将日历转换为字符串时，优先使用的语言标识列表；第二个参数：指定日历的类型；第三个参数：指定是12小时制还是24小时制
            Calendar calendarHebrew = new Calendar(new[] { "zh-CN" }, CalendarIdentifiers.Hebrew, ClockIdentifiers.TwentyFourHour);
            lblMsg.Text += "Gregorian Day: " + calendarDefault.DayAsString(); // 公历的日期
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Hebrew Day: " + calendarHebrew.DayAsString(); // 希伯来历的日期
            // Calendar 还有很多属性和方法，不再一一介绍，需要时查 msdn
        }
    }
}
