/*
 * ViewModel 层
 *
 * 注：为了方便使用，此例对 ICommand 做了一层封装。如果需要了解比较原始的 MVVM 实现请参见 http://www.cnblogs.com/webabcd/archive/2013/08/29/3288304.html
 */

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows10.MVVM.Model;

namespace Windows10.MVVM.ViewModel1
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        // 用于提供 Products 数据
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                RaisePropertyChanged(nameof(Products));
            }
        }

        // 用于“添加”和“查询”的 Product 对象
        private Product _product;
        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                RaisePropertyChanged(nameof(Product));
            }
        }

        // 数据库对象
        private ProductDatabase _context = null;

        public ProductViewModel()
        {
            _context = new ProductDatabase();

            Product = new Product();
            Products = new ObservableCollection<Product>(_context.GetProducts());
        }


        private MyCommand _getProductsCommand;
        public MyCommand GetProductsCommand
        {
            get
            {
                return _getProductsCommand ?? (_getProductsCommand = new MyCommand
                  ((object obj) =>
                  {
                      // 从 Model 层获取数据
                      Products = new ObservableCollection<Product>(_context.GetProducts(Product.Name, Product.Category));
                  },
                  null));
            }
        }

        private MyCommand _addProductCommand;
        public MyCommand AddProductCommand
        {
            get
            {
                return _addProductCommand ?? (_addProductCommand = new MyCommand
                  ((object obj) =>
                  {
                      // 在 Model 层添加一条数据
                      Product newProduct = _context.Add(Product.Name, Product.Category);

                      // 更新 ViewModel 层数据
                      Products.Insert(0, newProduct);
                  },
                  null));
            }
        }

        private MyCommand _updateProductCommand;
        public MyCommand UpdateProductCommand
        {
            get
            {
                return _updateProductCommand ?? (_updateProductCommand = new MyCommand
                  ((object obj) =>
                  {
                      // 通过 CommandParameter 传递过来的数据
                      Product product = obj as Product;

                      // 更新 ViewModel 层数据
                      product.Name = product.Name + "U";
                      product.Category = product.Category + "U";

                      // 更新 Model 层数据
                      _context.Update(product);
                  },
                  // 对应 ICommand 的 CanExecute()，如果返回 false 则对应的 ButtonBase 将变为不可用
                  (object obj) => obj != null));
            }
        }

        private MyCommand _deleteProductCommand;
        public MyCommand DeleteProductCommand
        {
            get
            {
                return _deleteProductCommand ?? (_deleteProductCommand = new MyCommand
                  ((object obj) =>
                  {
                      // 通过 CommandParameter 传递过来的数据
                      Product product = obj as Product;

                      // 更新 Model 层数据
                      _context.Delete(product);

                      // 更新 ViewModel 层数据
                      Products.Remove(product);
                  },
                  // 对应 ICommand 的 CanExecute()，如果返回 false 则对应的 ButtonBase 将变为不可用
                  (object obj) => obj != null));
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