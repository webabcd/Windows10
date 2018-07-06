/*
 * 用于演示自定义 StateTrigger
 *
 *
 * StateTriggerBase - 自定义 StateTrigger 需要继承此基类
 *     SetActive(Boolean IsActive) - 调用此方法，传递 true 则应用对应的 VisualState；传递 false 则取消对应的 VisualState
 *
 *
 * 此类的作用：当指定的 FrameworkElement 触发 PointerPressedEvent 事件时，根据 PointerDeviceType 的不同触发不同的 VisualState
 * 注：如果 TargetElement 属性或 PointerType 属性需要绑定的话，别忘了将其定义为依赖属性
 */

using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.UI.VisualState
{
    public class MyInputTypeStateTrigger : StateTriggerBase
    {
        private FrameworkElement _targetElement;
        private PointerDeviceType _lastPointerType, _triggerPointerType;
        private PointerEventHandler _pointerEventHandler;

        // 指定的 FrameworkElement
        public FrameworkElement TargetElement
        {
            get
            {
                return _targetElement;
            }
            set
            {
                if (_pointerEventHandler == null)
                {
                    _pointerEventHandler = new PointerEventHandler(_targetElement_PointerPressed);
                }

                if (_targetElement != null)
                {
                    _targetElement.RemoveHandler(FrameworkElement.PointerPressedEvent, _pointerEventHandler);
                }

                _targetElement = value;

                // 监听 FrameworkElement 的 PointerPressedEvent 事件
                _targetElement.AddHandler(FrameworkElement.PointerPressedEvent, _pointerEventHandler, true);

                // 这么写有问题，因为点击 button 时不会触发此事件
                // _targetElement.PointerPressed += _targetElement_PointerPressed;
            }
        }

        private void _targetElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _lastPointerType = e.Pointer.PointerDeviceType;
            UpdateTrigger();
        }

        // 指定的 PointerDeviceType（Touch, Pen, Mouse）
        public PointerDeviceType PointerType
        {
            get
            {
                return _triggerPointerType;
            }
            set
            {
                _triggerPointerType = value;
            }
        }

        public void UpdateTrigger()
        {
            // 指定的 FrameworkElement 触发 PointerPressedEvent 事件后，其 PointerDeviceType 如果和指定的 PointerDeviceType 一致，则触发对应的 VisualState
            SetActive(_triggerPointerType == _lastPointerType);
        }
    }
}

