/*
 * AutoSuggestBox - 自动建议文本框（继承自 ItemsControl, 请参见 /Controls/CollectionControl/ItemsControlDemo/）
 *     TextMemberPath - 建议框中的对象映射到文本框中时的字段名（如果建议框中显示的只是字符串，就不用设置这个了）
 *     UpdateTextOnSelect - 单击建议框中的项时，是否将其中的 TextMemberPath 指定的值赋值给文本框
 *     SuggestionChosen - 在建议框（即下拉部分）中选择了某一项后触发的事件
 *     TextChanged - 文本框中的输入文本发生变化时触发的事件
 *     QuerySubmitted - 用户提交查询时触发的事件（可以通过回车提交，可以通过点击文本框右侧的图标提交，可以通过在下拉框中选择某一项提交）
 *
 * 注：SearchBox 弃用了
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.TextControl
{
    public sealed partial class AutoSuggestBoxDemo : Page
    {
        public ObservableCollection<SuggestionModel> Suggestions { get; set; } = new ObservableCollection<SuggestionModel>();
     
        public AutoSuggestBoxDemo()
        {
            this.InitializeComponent();
            
            // 数据源有 Title 字段和 ImageUrl 字段，当用户在建议框中选中某一个对象时，会将 Title 字段指定的值赋值给文本框
            autoSuggestBox.TextMemberPath = "Title";
            // 用户选中建议框中的对象时，是否将 TextMemberPath 指定的值赋值给文本框
            autoSuggestBox.UpdateTextOnSelect = true;

            autoSuggestBox.TextChanged += autoSuggestBox_TextChanged;
            autoSuggestBox.SuggestionChosen += autoSuggestBox_SuggestionChosen;
            autoSuggestBox.QuerySubmitted += AutoSuggestBox_QuerySubmitted;
        }

        void autoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // 因为用户的输入而使得 Text 发生变化
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                Suggestions.Clear();

                // 根据用户的输入，修改 AutoSuggestBox 的数据源
                for (int i = 0; i < 10; i++)
                {
                    Suggestions.Add(new SuggestionModel()
                    {
                        Title = (sender.Text + "_" + i.ToString()),
                        ImageUrl = "/Assets/StoreLogo.png"
                    });
                }
            }
            // 通过代码使 Text 发生变化
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
            {

            }
            // 因为用户在建议框（即下拉部分）选择了某一项而使得 Text 发生变化
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen)
            {

            }
        }

        void autoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // AutoSuggestBoxSuggestionChosenEventArgs
            //     SelectedItem - 在建议框（即下拉部分）中选择的对象
            lblMsg1.Text = "选中的是：" + ((SuggestionModel)args.SelectedItem).Title;
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            lblMsg2.Text = $"用户提交了查询, 查询内容: {args.QueryText}";
            if (args.ChosenSuggestion != null)
            {
                lblMsg2.Text += Environment.NewLine;
                lblMsg2.Text += $"用户提交了查询（通过在下拉框中选择某一项的方式提交）, 查询内容: {((SuggestionModel)args.ChosenSuggestion).Title}";
            }
        }
    }

    public class SuggestionModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
