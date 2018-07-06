/*
 * 用于简化 INotifyPropertyChanged 的实现
 * 
 * 
 * 注：这里用 [CallerMemberName] 来获取字段名，在 C# 6.0 中也可以用新增的 nameof 表达式
 */

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Windows10.Common
{
    /// <summary>
    /// 用于简化模型的 <see cref="INotifyPropertyChanged"/> 实现。
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 针对属性更改通知的多播事件。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 检查属性是否已与所需值匹配。仅当需要时才设置
        /// 该属性并通知侦听器。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="storage">对具有 getter 和 setter 的属性的引用。</param>
        /// <param name="value">属性的所需值。</param>
        /// <param name="propertyName">用于通知侦听器的属性的名称。此
        /// 值是可选的，可以在从支持 CallerMemberName 的编译器调用时
        /// 自动提供。</param>
        /// <returns>如果更改了值，则为 true，如果现有值与所需值匹配，
        /// 则为 false。</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 向侦听器通知已更改了某个属性值。
        /// </summary>
        /// <param name="propertyName">用于通知侦听器的属性的名称。此
        /// 值是可选的，可以在从支持
        /// <see cref="CallerMemberNameAttribute"/> 的编译器调用时自动提供。</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
