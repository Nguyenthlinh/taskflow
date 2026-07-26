using Microsoft.EntityFrameworkCore;
using TaskApi.Models;


namespace TaskApi.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor này bắt buộc khi dùng với Web API
        // (Khác Console App — ở đây không override OnConfiguring)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        // DbSet<T> đại diện cho 1 bảng. Khi bạn gọi db.Tasks.Add(...) thì EF Core sẽ sinh ra câu INSERT INTO Tasks...
    }
}
