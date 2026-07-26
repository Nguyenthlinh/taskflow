using EFShopApp;
using EFShopApp.Models;
using Microsoft.EntityFrameworkCore;
Console.OutputEncoding = System.Text.Encoding.UTF8;

// KHỞI TẠO DBCONTEXT
// Chữ 'using' đảm bảo kết nối Database sẽ tự động được đóng lại khi chạy xong
using var db = new AppDbContext();

// ==========================================
// YÊU CẦU 1: CREATE (Thêm Danh mục)
// ==========================================
Console.WriteLine("1. Đang thêm danh mục...");
var cat1 = new Category { Name = "Công nghệ" };
var cat2 = new Category { Name = "Gia dụng" };

//// Thêm vào DbSet
//db.Categories.Add(cat1);
//db.Categories.Add(cat2);

//// Bắt buộc gọi SaveChanges để nó sinh ra lệnh SQL INSERT INTO và chạy
//db.SaveChanges();
//Console.WriteLine("=> Đã thêm danh mục xong!\n");


//Console.WriteLine("2. Đang thêm sản phẩm...");
//var p1 = new Product { Name = "iPhone 15", Price = 25000000, CategoryId = cat1.Id };
//var p2 = new Product { Name = "Macbook Air", Price = 20000000, CategoryId = cat1.Id };
//var p3 = new Product { Name = "Nồi chiên không dầu", Price = 2000000, CategoryId = cat2.Id };
//db.Products.Add(p1);
//db.Products.Add(p2);
//db.Products.Add(p3);
//db.SaveChanges();
//Console.WriteLine("=> Đã thêm sản phẩm xong!\n");
//Console.WriteLine("5. Đang thử xóa danh mục Gia dụng...");
//// Tìm danh mục tên "Gia dụng"
//var giaDungCat = db.Categories.FirstOrDefault(c => c.Name == "Công nghệ");
//if (giaDungCat != null)
//{
//    db.Categories.Remove(giaDungCat); // Ra lệnh xóa

//    try
//    {
//        db.SaveChanges(); // Cố gắng chạy lệnh DELETE
//        Console.WriteLine("=> Đã xóa thành công!");
//    }
//    catch (Exception ex)
//    {
//        // 🚨 CHỖ NÀY SẼ VĂNG LỖI!
//        // Vì danh mục "Gia dụng" đang chứa cái "Nồi chiên không dầu" bên bảng Product.
//        // CSDL sẽ báo lỗi Ràng buộc khóa ngoại (Foreign Key Constraint) không cho bạn xóa cha khi còn con.
//        Console.WriteLine($"=> XÓA THẤT BẠI. Lỗi từ CSDL: {ex.InnerException?.Message ?? ex.Message}");
//    }
//}
Console.WriteLine("3. Danh sách tất cả sản phẩm:");
// Gọi .Include(p => p.Category) để EF Core lấy kèm thông tin Danh mục
var allProducts = db.Products.Include(p => p.Category).ToList();
foreach (var p in allProducts)
{
    // p.Category.Name lấy ra được là nhờ hàm Include() ở trên
    Console.WriteLine($"[{p.Category.Name}] {p.Name} - {p.Price:N0} VNĐ");
}
Console.WriteLine();
