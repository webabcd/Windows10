/*
 * 自定义 ContentPresenter 实现类似 GridViewItemPresenter 和 ListViewItemPresenter 的效果
 */

using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    class MyItemPresenter : ContentPresenter
    {
        Panel _container = null; // item 的容器（即在 DataTemplate 中定义的根元素，在示例 MyItemPresenterDemo.xaml 中用的是 Grid）
        Rectangle _pointerOverBorder = null; // 鼠标经过 item 时覆盖在 item 上的 rectangle
        Rectangle _focusVisual = null; // 选中 item 时覆盖在 item 上的 rectangle

        Storyboard _pointerDownStoryboard = null; // 鼠标按下时的动画
        Storyboard _pointerUpStoryboard = null; // 鼠标抬起时的动画

        public MyItemPresenter()
            : base()
        {
            base.Margin = new Thickness(10);
        }

        // override OnApplyTemplate() - 应用控件模板时调用
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = (Panel)VisualTreeHelper.GetChild(this, 0);
        }

        // override GoToElementStateCore() - VisualState 转换时调用（此方法仅在自定义 ContentPresenter 并将其应用于 GridView 或 ListView 的 ItemContainerStyle 时才会被调用）
        //     stateName - VisualState 的名字
        //     useTransitions - 是否使用 VisualTransition 过渡效果
        protected override bool GoToElementStateCore(string stateName, bool useTransitions)
        {
            base.GoToElementStateCore(stateName, useTransitions);

            switch (stateName)
            {
                // 正常状态
                case "Normal":
                    HidePointerOverVisuals();
                    HideFocusVisuals();
                    if (useTransitions)
                    {
                        StopPointerDownAnimation();
                    }
                    break;

                // 选中状态
                case "Selected":
                case "PointerFocused":
                    ShowFocusVisuals();
                    if (useTransitions)
                    {
                        StopPointerDownAnimation();
                    }
                    break;

                // 取消选中状态
                case "Unfocused":
                    HideFocusVisuals();
                    break;

                // 鼠标经过状态
                case "PointerOver":
                    ShowPointerOverVisuals();
                    if (useTransitions)
                    {
                        StopPointerDownAnimation();
                    }
                    break;

                // 鼠标点击状态
                case "Pressed":
                case "PressedSelected":
                    if (useTransitions)
                    {
                        StartPointerDownAnimation();
                    }
                    break;

                default: break;
            }

            return true;
        }

        private void StartPointerDownAnimation()
        {
            if (_pointerDownStoryboard == null)
                CreatePointerDownStoryboard();

            _pointerDownStoryboard.Begin();
        }

        private void StopPointerDownAnimation()
        {
            if (_pointerUpStoryboard == null)
                CreatePointerUpStoryboard();

            _pointerUpStoryboard.Begin();
        }

        private void ShowFocusVisuals()
        {
            if (!FocusElementsAreCreated())
                CreateFocusElements();

            _focusVisual.Opacity = 1;
        }

        private void HideFocusVisuals()
        {
            if (FocusElementsAreCreated())
                _focusVisual.Opacity = 0;
        }

        private void ShowPointerOverVisuals()
        {
            if (!PointerOverElementsAreCreated())
                CreatePointerOverElements();

            _pointerOverBorder.Opacity = 1;
        }

        private void HidePointerOverVisuals()
        {
            if (PointerOverElementsAreCreated())
                _pointerOverBorder.Opacity = 0;
        }

        private void CreatePointerDownStoryboard()
        {
            /*
             * 用这种方式为 item 实现鼠标按下的效果会报错（Attempted to read or write protected memory. This is often an indication that other memory is corrupt.），不知道为什么
             * PointerDownThemeAnimation pointerDownAnimation = new PointerDownThemeAnimation();
             * Storyboard.SetTarget(pointerDownAnimation, _container);
             * Storyboard pointerDownStoryboard = new Storyboard();
             * pointerDownStoryboard.Children.Add(pointerDownAnimation);
             */

            DoubleAnimation da1 = new DoubleAnimation()
            {
                To = 0.9,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            DoubleAnimation da2 = new DoubleAnimation()
            {
                To = 0.9,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            Storyboard.SetTarget(da1, _container);
            Storyboard.SetTargetProperty(da1, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(da2, _container);
            Storyboard.SetTargetProperty(da2, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)");
            if (!(_container.RenderTransform is TransformGroup))
            {
                TransformGroup Group = new TransformGroup();
                Group.Children.Add(new ScaleTransform());
                _container.RenderTransform = Group;
                _container.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            _pointerDownStoryboard = new Storyboard();
            _pointerDownStoryboard.Children.Add(da1);
            _pointerDownStoryboard.Children.Add(da2);
            _pointerDownStoryboard.Begin();
        }

        private void CreatePointerUpStoryboard()
        {
            DoubleAnimation da1 = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            DoubleAnimation da2 = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(100)
            };
            Storyboard.SetTarget(da1, _container);
            Storyboard.SetTargetProperty(da1, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(da2, _container);
            Storyboard.SetTargetProperty(da2, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)");
            if (!(_container.RenderTransform is TransformGroup))
            {
                TransformGroup Group = new TransformGroup();
                Group.Children.Add(new ScaleTransform());
                _container.RenderTransform = Group;
                _container.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            _pointerUpStoryboard = new Storyboard();
            _pointerUpStoryboard.Children.Add(da1);
            _pointerUpStoryboard.Children.Add(da2);
            _pointerUpStoryboard.Begin();
        }

        private void CreatePointerOverElements()
        {
            _pointerOverBorder = new Rectangle();
            _pointerOverBorder.IsHitTestVisible = false;
            _pointerOverBorder.Opacity = 0;
            // 这里把颜色写死了，仅为演示用，实际写的时候要摘出来写成依赖属性
            _pointerOverBorder.Fill = new SolidColorBrush(Color.FromArgb(0x50, 0x50, 0x50, 0x50));

            _container.Children.Insert(_container.Children.Count, _pointerOverBorder);
        }

        private void CreateFocusElements()
        {
            _focusVisual = new Rectangle();
            _focusVisual.IsHitTestVisible = false;
            _focusVisual.Height = 10;
            _focusVisual.VerticalAlignment = VerticalAlignment.Bottom;
            // 这里把颜色写死了，仅为演示用，实际写的时候要摘出来写成依赖属性
            _focusVisual.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0x0, 0x0));

            _container.Children.Insert(0, _focusVisual);
        }

        private bool FocusElementsAreCreated()
        {
            return _focusVisual != null;
        }

        private bool PointerOverElementsAreCreated()
        {
            return _pointerOverBorder != null;
        }
    }
}
