/*
 * Model 层的实体类，如果需要通知则需要实现 INotifyPropertyChanged 接口
 */

using System.ComponentModel;

namespace Windows10.MVVM.Model
{
    public class Product : INotifyPropertyChanged
    {
        public Product()
        {
            ProductId = 0;
            Name = "";
            Category = "";
        }

        private int _productId;
        public int ProductId
        {
            get { return _productId; }
            set
            {
                _productId = value;
                RaisePropertyChanged(nameof(ProductId));
            }
        }

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

        private string _category;
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged(nameof(Category));
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
