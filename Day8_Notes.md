# 📚 DAY 8 – Entity Framework Core (Code First)

---

### 1. KIẾN THỨC NỀN TẢNG
- **Entity Framework Core (EF Core)**: Là một ORM (Object-Relational Mapper) giúp lập trình viên C# tương tác với Database bằng Code C# (class, LINQ) thay vì phải viết câu lệnh SQL thuần.
- **Code First**: Phương pháp lập trình thiết kế các Class C# trước, sau đó để EF Core tự động sinh ra Database.

### 2. CÁC THÀNH PHẦN CHÍNH

#### 🔸 Entity (Thực thể)
Là các Class đại diện cho bảng (Table) trong Database.
```csharp
public class Category
{
    // Id tự động trở thành Khóa chính (Primary Key)
    public int Id { get; set; } 
    public string Name { get; set; }
    
    // Mối quan hệ 1-Nhiều: 1 Danh mục chứa nhiều Sản phẩm
    public List<Product> Products { get; set; } = new();
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    
    // Khóa ngoại (Foreign Key)
    public int CategoryId { get; set; }
    
    // Navigation Property: Trỏ tới Category cụ thể
    public Category Category { get; set; }
}
```

#### 🔸 DbContext (Cầu nối)
Là Class quan trọng nhất, kế thừa từ `Microsoft.EntityFrameworkCore.DbContext`. Nó cấu hình kết nối tới Database và khai báo các bảng.
```csharp
public class AppDbContext : DbContext
{
    // Đại diện cho 2 bảng trong DB
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    // Cấu hình chuỗi kết nối
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Server=.\\SQLEXPRESS; Database=EFShopDB; Trusted_Connection=True; TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }
}
```

### 3. CÁC LỆNH MIGRATION CƠ BẢN
Để sinh ra Database từ Code C#, ta cần cài 2 thư viện NuGet (nhớ chọn phiên bản tương thích với .NET hiện tại, ví dụ .NET 9 thì cài bản `9.0.x`):
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

Mở cửa sổ **Package Manager Console** và dùng 2 lệnh sau:

1. **`Add-Migration [Tên_Migration]`**: 
   - Lệnh này quét các Class C# và tạo ra một file code so sánh sự thay đổi (giống như "bản vẽ thi công").
   - Ví dụ: `Add-Migration InitDB`

2. **`Update-Database`**: 
   - Lệnh này đọc file Migration vừa sinh ra và áp dụng (thực thi) xuống SQL Server để tạo/sửa Database thật.
