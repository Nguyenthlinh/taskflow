using Microsoft.EntityFrameworkCore;
using ProductWebApi.Models;

namespace ProductWebApi.Data
{
    public class AppDbContext: DbContext
    {
        // Constructor này bắt buộc phải có khi dùng với Web API
        // Khác với Console App (override OnConfiguring), ở đây config được inject từ bên ngoài
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; } 

    }
}
