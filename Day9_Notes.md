# 📚 DAY 9 – EF Core: CRUD bằng C# (Không cần viết SQL)

---

### QUY TẮC VÀNG: Mọi thay đổi đều phải gọi `SaveChanges()`
Khi bạn gọi `.Add()`, `.Remove()`, hoặc sửa property của object, EF Core chỉ "ghi nhớ" kế hoạch trong RAM.
**Phải gọi `db.SaveChanges()` thì EF Core mới thực sự sinh ra câu SQL và chạy xuống Database.**

---

### 1. SETUP (Mở kết nối)
```csharp
// 'using' giúp tự động đóng kết nối khi chạy xong
using var db = new AppDbContext();
```

---

### 2. CREATE (Thêm dữ liệu — INSERT INTO)
```csharp
var newCategory = new Category { Name = "Công nghệ" };
db.Categories.Add(newCategory); // Chỉ lưu trong RAM
db.SaveChanges();               // Thực sự INSERT vào SQL Server

// SAU SaveChanges, CSDL đã cấp phát Id tự động, ta có thể đọc:
Console.WriteLine(newCategory.Id); // In ra: 1, 2, 3...
```

---

### 3. READ (Đọc dữ liệu — SELECT)
```csharp
// Lấy toàn bộ
var all = db.Products.ToList();

// Lọc bằng LINQ (EF Core tự dịch sang WHERE)
var cheap = db.Products.Where(p => p.Price < 5000000).ToList();

// Tìm theo Id (nhanh nhất)
var p = db.Products.Find(1);

// Tìm 1 phần tử theo điều kiện (trả null nếu không thấy)
var iphone = db.Products.FirstOrDefault(p => p.Name.Contains("iPhone"));
```

**Lấy kèm dữ liệu liên quan (Thay thế INNER JOIN):**
```csharp
using Microsoft.EntityFrameworkCore; // Bắt buộc phải using này

// .Include() kêu EF Core JOIN tự động sang bảng liên kết
var products = db.Products
    .Include(p => p.Category) // Lấy kèm thông tin Danh mục
    .ToList();

// Giờ mới có thể truy cập p.Category.Name
foreach (var p in products)
    Console.WriteLine($"[{p.Category.Name}] {p.Name}");
```

---

### 4. UPDATE (Sửa dữ liệu — UPDATE SET)
```csharp
// Quy trình 3 bước: TÌM → SỬA → LƯU
var p = db.Products.Find(1);
if (p != null)
{
    p.Price = 30000000; // Sửa trực tiếp property
    db.SaveChanges();   // EF Core tự sinh câu UPDATE
}
```

---

### 5. DELETE (Xóa dữ liệu — DELETE FROM)
```csharp
// Quy trình 3 bước: TÌM → REMOVE → LƯU
var p = db.Products.Find(5);
if (p != null)
{
    db.Products.Remove(p);
    db.SaveChanges();
}
```

---

### ⚠️ LỖI FOREIGN KEY KHI XÓA (Cực hay gặp!)
**Tình huống:** Xóa Danh mục "Gia dụng" đang chứa sản phẩm "Nồi chiên không dầu".

**CSDL sẽ báo lỗi kiểu này:**
```
The DELETE statement conflicted with the REFERENCE constraint...
```

**Lý do:** Foreign Key Constraint bảo vệ tính toàn vẹn dữ liệu.
CSDL không cho xóa bảng cha khi bảng con còn đang trỏ vào nó.

**Cách fix — 2 lựa chọn:**
```csharp
// CÁCH 1: Xóa con trước, rồi mới xóa cha
db.Products.RemoveRange(db.Products.Where(p => p.CategoryId == cat.Id));
db.SaveChanges();
db.Categories.Remove(cat);
db.SaveChanges();

// CÁCH 2: Bọc trong try-catch để thông báo lỗi thay vì crash app
try { db.SaveChanges(); }
catch (Exception ex) { Console.WriteLine($"Lỗi: {ex.InnerException?.Message}"); }
```

---

### SO SÁNH SQL THUẦN vs EF CORE

| Hành động | SQL thuần | EF Core C# |
|-----------|-----------|------------|
| Thêm | `INSERT INTO Products...` | `db.Products.Add(p); db.SaveChanges();` |
| Lấy tất cả | `SELECT * FROM Products` | `db.Products.ToList()` |
| Lọc | `WHERE Price > 100000` | `.Where(p => p.Price > 100000)` |
| INNER JOIN | `INNER JOIN Categories ON ...` | `.Include(p => p.Category)` |
| Sửa | `UPDATE Products SET Price = ...` | `p.Price = ...; db.SaveChanges();` |
| Xóa | `DELETE FROM Products WHERE Id = 5` | `db.Products.Remove(p); db.SaveChanges();` |
