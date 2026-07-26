using EFShopApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFShopApp
{
    public class AppDbContext : DbContext
    {
        // 2 dòng này đại diện cho 2 bảng
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        // Cấu hình chuỗi kết nối
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Thay ".\SQLEXPRESS" bằng tên Server của bạn (vào SSMS copy) nếu cần
            string connectionString = "Server=.\\HoaiLinh; Database=EFShopDB; Trusted_Connection=True; TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);

        }
    }
}
