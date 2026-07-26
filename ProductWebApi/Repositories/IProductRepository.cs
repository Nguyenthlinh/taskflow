using ProductWebApi.Models;
namespace ProductWebApi.Repositories
{
    // Interface — chỉ định nghĩa "làm gì", không quan tâm "làm như thế nào"
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task SaveAsync();
    }
}
