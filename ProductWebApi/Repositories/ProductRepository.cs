using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Models;

namespace ProductWebApi.Repositories

{
    // Implement interface — chỉ được phép đụng vào DbContext ở đây

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetAllAsync()
            => await _db.Products.ToListAsync();
        public async Task<Product?> GetByIdAsync(int id)
            => await _db.Products.FindAsync(id);
        public async Task AddAsync(Product product)
            => await _db.Products.AddAsync(product);
        public Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Product product)
        {
            _db.Products.Remove(product);
            return Task.CompletedTask;
        }
        public async Task SaveAsync()
            => await _db.SaveChangesAsync();
    }
}
