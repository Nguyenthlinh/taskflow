# 📚 DAY 11 – Layered Architecture (Service + Repository Pattern)

---

### 1. TẠI SAO CẦN TÁCH TẦNG?

| Tầng | Trách nhiệm | KHÔNG được làm |
|------|-------------|---------------|
| **Controller** | Nhận request, trả response | Truy vấn DB trực tiếp |
| **Service** | Xử lý business logic, validate | Truy vấn DB trực tiếp |
| **Repository** | Chỉ truy vấn DB | Chứa business logic |

---

### 2. CẤU TRÚC PROJECT CHUẨN

```
Controllers/
    ProductsController.cs   ← Gọi IProductService
Services/
    IProductService.cs      ← Interface định nghĩa contract
    ProductService.cs       ← Implement, chứa business logic
Repositories/
    IProductRepository.cs   ← Interface định nghĩa contract
    ProductRepository.cs    ← Implement, chỉ đụng DbContext ở đây
Data/
    AppDbContext.cs
Models/
    Product.cs
Program.cs
```

---

### 3. LUỒNG DI CHUYỂN CỦA MỘT REQUEST

```
HTTP Request (GET /api/products)
    │
    ▼
ProductsController
    │  _service.GetAllAsync()
    ▼
ProductService
    │  _repo.GetAllAsync()
    ▼
ProductRepository
    │  _db.Products.ToListAsync()
    ▼
SQL Server → Trả JSON ngược lên
```

---

### 4. DEPENDENCY INJECTION — ĐĂNG KÝ TRONG PROGRAM.CS

```csharp
// AddScoped = tạo 1 instance mới cho mỗi HTTP request
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Khi Controller khai báo IProductService trong constructor,
// ASP.NET Core tự động tìm và inject ProductService vào
```

**3 kiểu vòng đời (Lifetime):**
| Lifetime | Tạo instance khi nào |
|----------|---------------------|
| `AddScoped` | Mỗi HTTP Request (phổ biến nhất) |
| `AddTransient` | Mỗi lần được inject |
| `AddSingleton` | Chỉ tạo 1 lần, dùng mãi |

---

### 5. PROGRAM.CS ĐẦY ĐỦ

```csharp
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Repositories;
using ProductWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS; Database=ProductWebApiDB; Trusted_Connection=True; TrustServerCertificate=True;")
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

---

### 6. BUSINESS LOGIC TRONG SERVICE

```csharp
public async Task<Product> CreateAsync(Product product)
{
    // Validate tại đây, không phải trong Controller
    if (product.Price < 0)
        throw new ArgumentException("Giá tiền không được âm!");

    await _repo.AddAsync(product);
    await _repo.SaveAsync();
    return product;
}
```

Controller bắt exception từ Service và trả về HTTP Status Code phù hợp:
```csharp
catch (ArgumentException ex)
{
    return BadRequest(ex.Message); // 400
}
```
