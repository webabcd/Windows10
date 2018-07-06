/*
 * 开发一个自定义控件，用于演示控件模板和事件处理的相关知识点
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace MyControls
{
    /// <summary>
    /// 自定义控件
    /// </summary>
    public sealed class MyControl3 : Control
    {
        public MyControl3()
        {
            this.DefaultStyleKey = typeof(MyControl3);
        }

        // ApplyTemplate() - 强制加载控件模板，一般不用调用（因为控件模板会自动加载）。有一种使用场景是：当父控件应用控件模板时要求子控件必须先应用控件模板以便父控件使用时，则可以先调用子控件的此方法
        // GetTemplateChild() - 查找控件模板中的指定名字的元素
        // override OnApplyTemplate() - 应用控件模板时调用
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBlock textBlock = (TextBlock)GetTemplateChild("textBlock");
            if (this.Background is SolidColorBrush)
            {
                textBlock.Text = $"background: {((SolidColorBrush)this.Background).Color}";
            }

            VisualStateManager.GoToState(this, "Normal", false);
        }

        // override GoToElementStateCore() - VisualState 转换时调用（此方法仅在自定义 ContentPresenter 并将其应用于 GridView 或 ListView 的 ItemContainerStyle 时才会被调用）
        //     参见：/Controls/CollectionControl/ItemsControlDemo/MyItemPresenter.cs
        protected override bool GoToElementStateCore(string stateName, bool useTransitions)
        {
            return base.GoToElementStateCore(stateName, useTransitions);
        }


        // 在 Control 中有很多可 override 的事件处理方法，详见文档
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerOver", true);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }
    }
}