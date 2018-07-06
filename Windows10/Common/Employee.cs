using System;
using System.ComponentModel;

namespace Windows10.Common
{
    /// <summary>
    /// 一个实现了 INotifyPropertyChanged 接口的实体类
    /// </summary>
    public class Employee : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private int _age;
        public int Age
        {
            get { return _age; }
            set 
            {
                _age = value;
                RaisePropertyChanged(nameof(Age));
            }
        }

        private bool _isMale;
        public bool IsMale
        {
            get { return _isMale; }
            set
            {
                _isMale = value;
                RaisePropertyChanged(nameof(IsMale));
            }
        }

        // 用于演示事件绑定到方法
        public void ChangeName()
        {
            Name = "wanglei" + new Random().Next(1000, 10000).ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public override string ToString()
        {
            return $"ToString() - {this.Name}";
        }
    }
}
