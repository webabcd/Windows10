/*
 * 为了方便使用，把 ICommand 再封装一层
 */

using System;
using System.Windows.Input;

namespace Windows10.MVVM.ViewModel1
{
    public class MyCommand : ICommand
    {
        // 由 public void Execute(object parameter) 调用的委托
        public Action<object> MyExecute { get; set; }

        // 由 public bool CanExecute(object parameter) 调用的委托
        public Func<object, bool> MyCanExecute { get; set; }

        public MyCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.MyExecute = execute;
            this.MyCanExecute = canExecute;
        }

        // 需要发布此事件的话，则调用 RaiseCanExecuteChanged 方法即可
        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        // 用于决定当前绑定的 Command 能否被执行
        // parameter 是由 ButtonBase 的 CommandParameter 传递过来的
        // 如果返回 false 则对应的 ButtonBase 将变为不可用
        public bool CanExecute(object parameter)
        {
            return this.MyCanExecute == null ? true : this.MyCanExecute(parameter);
        }

        // 用于执行对应的命令，只有在 CanExecute() 返回 true 时才可以被执行
        // parameter 是由 ButtonBase 的 CommandParameter 传递过来的对象
        public void Execute(object parameter)
        {
            this.MyExecute(parameter);
        }
    }
}