/*
 * SecondaryViewHelper - 自定义的一个帮助类，用于简化 SecondaryView 的管理
 */

using System;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Windows10.UI.MultipleViews
{
    public class SecondaryViewHelper : INotifyPropertyChanged
    {
        // for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // 当前 SecondaryView 的 CoreDispatcher
        private CoreDispatcher _dispatcher;
        // 当前 SecondaryView 的 ApplicationView
        private ApplicationView _applicationView;

        // 当前 SecondaryView 的标题
        private string _title;
        // 当前 SecondaryView 的窗口标识
        private int _viewId;

        // 当前 SecondaryView 被引用的次数
        private int _refCount = 0;
        // 当前 SecondaryView 是否已经被释放
        private bool _released = false;

        // 禁止通过 new 实例化
        private SecondaryViewHelper(CoreWindow newWindow)
        {
            _dispatcher = newWindow.Dispatcher;
            _viewId = ApplicationView.GetApplicationViewIdForWindow(newWindow);

            _applicationView = ApplicationView.GetForCurrentView();

            RegisterForEvents();
        }

        // 实例化 SecondaryViewHelper
        public static SecondaryViewHelper CreateForCurrentView()
        {
            /*
             * CoreWindow.GetForCurrentThread() - 获取当前窗口的 CoreWindow
             */
            return new SecondaryViewHelper(CoreWindow.GetForCurrentThread());
        }
        private void RegisterForEvents()
        {
            /*
             * ApplicationView.GetForCurrentView() - 获取当前窗口的 ApplicationView
             * ApplicationView.Consolidated - 当前 app 存活着两个或两个以上的窗口时，此窗口关闭后触发的事件
             */
            ApplicationView.GetForCurrentView().Consolidated += SecondaryViewHelper_Consolidated;
        }

        private void UnregisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated -= SecondaryViewHelper_Consolidated;
        }

        private void SecondaryViewHelper_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            StopViewInUse();
        }

        // 当前 SecondaryView 开始使用了（与 StopViewInUse() 成对）
        // 因为每一个窗口都可以被同 app 的别的窗口调用，而每一个窗口又都是一个独立的线程，所以要做好线程处理
        public int StartViewInUse()
        {
            bool releasedCopy = false;
            int refCountCopy = 0;

            lock (this)
            {
                releasedCopy =_released;
                if (!_released)
                {
                    refCountCopy = ++_refCount;
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("this view is being disposed");
            }

            return refCountCopy;
        }

        // 当前 SecondaryView 结束使用了（与 StartViewInUse() 成对）
        // 因为每一个窗口都可以被同 app 的别的窗口调用，而每一个窗口又都是一个独立的线程，所以要做好线程处理
        public int StopViewInUse()
        {
            int refCountCopy = 0;
            bool releasedCopy = false;

            lock (this)
            {
                releasedCopy = _released;
                if (!_released)
                {
                    refCountCopy = --_refCount;
                    if (refCountCopy == 0)
                    {
                        // 当前 SecondaryView 不再被任何人需要了，清理之
                        var task = _dispatcher.RunAsync(CoreDispatcherPriority.Low, FinalizeRelease);
                    }
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("this view is being disposed");
            }

            return refCountCopy;
        }

        // 清理当前 SecondaryView
        private void FinalizeRelease()
        {
            bool justReleased = false;
            lock (this)
            {
                if (_refCount == 0)
                {
                    justReleased = true;
                    _released = true;
                }
            }

            if (justReleased)
            {
                UnregisterForEvents();

                // 触发 Released 事件
                OnReleased(EventArgs.Empty);
            }
        }

        // 定义 Released 事件
        public event EventHandler<EventArgs> Released;
        protected virtual void OnReleased(EventArgs e)
        {
            EventHandler<EventArgs> handler = Released;
            if (handler != null)
                handler(this, e);
        }

        public int Id
        {
            get
            {
                return _viewId;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Title)));
                    }
                }
            }
        }

        public bool IsReleased
        {
            get
            {
                return _released;
            }
        }

        public ApplicationView ApplicationView
        {
            get
            {
                return _applicationView;
            }
        }
    }
}