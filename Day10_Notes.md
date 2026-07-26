# 📚 DAY 10 – ASP.NET Core Web API

---

### 1. KIẾN TRÚC REST API

```
Client (Postman, React, Mobile)
         │  HTTP Request
         ▼
    Controller (Nhận request, trả response)
         │
         ▼
    DbContext (EF Core)
         │
         ▼
    SQL Server Database
```

---

### 2. HTTP METHODS (Phải nhớ thuộc)

| Method | Tương đương SQL | Dùng khi |
|--------|-----------------|---------|
| `GET` | SELECT | Lấy danh sách hoặc 1 bản ghi |
| `POST` | INSERT | Tạo mới |
| `PUT` | UPDATE | Cập nhật toàn bộ |
| `DELETE` | DELETE | Xóa |

---

### 3. CẤU TRÚC PROJECT CHUẨN

```
ProductWebApi/
├── Models/           ← Class đại diện cho bảng DB
│   └── Product.cs
├── Data/             ← DbContext (cầu nối DB)
│   └── AppDbContext.cs
├── Controllers/      ← Nhận HTTP Request, trả Response
│   └── ProductsController.cs
└── Program.cs        ← Đăng ký Services, cấu hình pipeline
```

---

### 4. PROGRAM.CS — Đăng ký dịch vụ

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();             // Kích hoạt Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();              // Cần cài NuGet: Swashbuckle.AspNetCore

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS; Database=ProductWebApiDB; Trusted_Connection=True; TrustServerCertificate=True;")
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();    // Truy cập tại: https://localhost:{port}/swagger
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

---

### 5. CONTROLLER — Cấu trúc chuẩn

```csharp
[ApiController]
[Route("api/[controller]")]   // URL: /api/products
public class ProductsController : ControllerBase  // BẮT BUỘC kế thừa ControllerBase
{
    private readonly AppDbContext _db;

    // Dependency Injection — EF Core tự inject vào
    public ProductsController(AppDbContext db) { _db = db; }

    [HttpGet]                    // GET /api/products
    [HttpGet("{id}")]            // GET /api/products/1
    [HttpPost]                   // POST /api/products
    [HttpPut("{id}")]            // PUT /api/products/1
    [HttpDelete("{id}")]         // DELETE /api/products/1
}
```

---

### 6. CÁC HÀM TRẢ RESPONSE (trong ControllerBase)

| Hàm | HTTP Code | Dùng khi |
|-----|-----------|---------|
| `Ok(data)` | 200 | Thành công, có data trả về |
| `NotFound(msg)` | 404 | Không tìm thấy bản ghi |
| `CreatedAtAction(...)` | 201 | Tạo mới thành công |
| `NoContent()` | 204 | Thành công, không có data (DELETE) |
| `BadRequest(msg)` | 400 | Request sai định dạng |

---

### 7. DBCONTEXT TRONG WEB API

```csharp
// Khác với Console App (override OnConfiguring),
// trong Web API phải dùng constructor injection:
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}
```

---

### 8. LỖI HAY GẶP

| Lỗi | Nguyên nhân | Fix |
|-----|-------------|-----|
| `AddSwaggerGen` not found | Thiếu NuGet package | Cài `Swashbuckle.AspNetCore` bản 6.x.x |
| `Ok()`, `NotFound()` not found | Class không kế thừa `ControllerBase` | Thêm `: ControllerBase` |
| Browser 404 khi vào localhost | Vào sai URL | Thêm `/swagger` vào cuối URL |
| `IDENTITY_INSERT is set to OFF` | POST gửi lên trường `id` có giá trị | Xóa trường `id` hoặc để `"id": 0` |
