using ProductWebApi.Models;
using ProductWebApi.Repositories;

namespace ProductWebApi.Services
{
    // Service chứa business logic, gọi Repository để truy vấn DB

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<Product>> GetAllAsync()
          => await _repo.GetAllAsync();
        public async Task<Product?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);
        public async Task<Product> CreateAsync(Product product)
        {
            // Ví dụ business logic: validate giá tiền
            if (product.Price < 0)
                throw new ArgumentException("Giá tiền không được âm!");
            await _repo.AddAsync(product);
            await _repo.SaveAsync();
            return product;
        }
        public async Task<Product?> UpdateAsync(int id, Product updated)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return null;
            product.Name = updated.Name;
            product.Price = updated.Price;
            product.Category = updated.Category;
            await _repo.UpdateAsync(product);
            await _repo.SaveAsync();
            return product;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;
            await _repo.DeleteAsync(product);
            await _repo.SaveAsync();
            return true;
        }


    }
}
