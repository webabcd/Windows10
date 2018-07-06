/*
 * Model 层的数据持久化操作（本地或远程）
 * 
 * 本例只是一个演示
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows10.MVVM.Model
{
    public class ProductDatabase
    {
        private List<Product> _products = null;

        public List<Product> GetProducts()
        {
            if (_products == null)
            {
                Random random = new Random();

                _products = new List<Product>();

                for (int i = 0; i < 100; i++)
                {
                    _products.Add
                    (
                        new Product
                        {
                            ProductId = i,
                            Name = "Name" + i.ToString().PadLeft(4, '0'),
                            Category = "Category" + (char)random.Next(65, 91)
                        }
                    );
                }
            }

            return _products;
        }

        public List<Product> GetProducts(string name, string category)
        {
            return GetProducts().Where(p => p.Name.Contains(name) && p.Category.Contains(category)).ToList();
        }

        public void Update(Product product)
        {
            var oldProduct = _products.Single(p => p.ProductId == product.ProductId);
            oldProduct = product;
        }

        public Product Add(string name, string category)
        {
            Product product = new Product();
            product.ProductId = _products.Max(p => p.ProductId) + 1;
            product.Name = name;
            product.Category = category;

            _products.Insert(0, product);

            return product;
        }

        public void Delete(Product product)
        {
            _products.Remove(product);
        }
    }
}
