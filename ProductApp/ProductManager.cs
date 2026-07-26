using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp
{
    class ProductManager
    {
        private List<Product> _products = new();

        // Add a product to the list
        public void AddProduct(Product p)
        {
            _products.Add(p);
        }
        //tim theo Id
        public Product GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new ProductNotFoundException(id);

            return product;
        }
        //lọc theo category
        public List<Product> GetByCategory(string category)
        {
            return _products.Where<Product>(p => p.Category == category).ToList();

        }
        //N sản phẩm mắc nhất
        public List<Product> GetTopNExpensive(int n)
        {
            return _products.OrderByDescending(p => p.Price).Take(n).ToList();
        }
        //thống kê theo category dùng GroupBy
        public void  PrintSumPriceByCategory()
        {
            var groups = _products.GroupBy(p => p.Category);
            foreach (var group in groups)
            {
                int count = group.Count();
                double total = group.Sum(p => p.Price);
                Console.WriteLine($"{group.Key}: {count} sản phẩm | Tổng: {total:N0} VNĐ");
            }
        }
    }
}
