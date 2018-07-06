/*
 * ViewModel 层
 */

using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows10.MVVM.Model;

namespace Windows10.MVVM.ViewModel2
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


        public void GetProducts(object sender, RoutedEventArgs e)
        {
            // 从 Model 层获取数据
            Products = new ObservableCollection<Product>(_context.GetProducts(Product.Name, Product.Category));
        }

        public void AddProduct(object sender, RoutedEventArgs e)
        {
            // 在 Model 层添加一条数据
            Product newProduct = _context.Add(Product.Name, Product.Category);

            // 更新 ViewModel 层数据
            Products.Insert(0, newProduct);
        }

        public void UpdateProduct(object sender, RoutedEventArgs e)
        {
            Product product = ((FrameworkElement)sender).Tag as Product;

            // 更新 ViewModel 层数据
            product.Name = product.Name + "U";
            product.Category = product.Category + "U";

            // 更新 Model 层数据
            _context.Update(product);
        }

        public void DeleteProduct(object sender, RoutedEventArgs e)
        {
            Product product = ((FrameworkElement)sender).Tag as Product;

            // 更新 Model 层数据
            _context.Delete(product);

            // 更新 ViewModel 层数据
            Products.Remove(product);
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
