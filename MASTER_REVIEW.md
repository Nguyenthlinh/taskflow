# 🧠 MASTER REVIEW – Toàn Bộ Kiến Thức Backend .NET (14 Ngày)

> **Cách dùng file này:** Đọc từ trên xuống, nếu nhớ rồi thì bỏ qua, nếu quên thì đọc kỹ phần đó trước khi code.

---

## 📌 MỤC LỤC NHANH

| Ngày | Chủ đề | Keyword cần nhớ |
|------|--------|-----------------|
| Day 1–2 | C# Syntax + OOP Class | `List<T>`, Constructor, `{ get; set; }`, `private` |
| Day 3 | Inheritance + Interface | `:`, `virtual/override`, `abstract`, `interface` |
| Day 4–5 | Collections + Exception + Async | `Dictionary`, `GroupBy`, `try/catch`, `async/await` |
| Day 6–7 | SQL Server | `CREATE TABLE`, `INSERT`, `SELECT WHERE`, `JOIN`, `GROUP BY` |
| Day 8–9 | Entity Framework Core | `DbContext`, `Migration`, `.Include()`, `SaveChanges()` |
| Day 10 | ASP.NET Core Web API | Controller, HTTP Methods, Swagger |
| Day 11 | Layered Architecture | Repository → Service → Controller |
| Day 12 | DTO + Validation | `ProductCreateDto`, `[Required]`, `[Range]`, Mapper |
| Day 13 | JWT Authentication | Register, Login, `[Authorize]`, Token |
| Day 14 | Mini Project | Kết hợp tất cả |

---

# 🔷 PHẦN 1: C# NỀN TẢNG (Day 1–2)

## Cú pháp hay dùng
```csharp
// String interpolation
Console.WriteLine($"Tên: {name}, Điểm: {gpa:F2}");

// Null coalescing — nếu null thì dùng giá trị mặc định
string input = Console.ReadLine() ?? "0";

// Parse an toàn — không crash khi nhập sai
if (double.TryParse(input, out double d)) { ... }

// Loop không biết trước số lần
while (true)
{
    string s = Console.ReadLine();
    if (s == "exit") break;
}
```

## Class và Object
```csharp
// Khai báo class
public class Student
{
    // Properties
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double GPA { get; set; }

    // Constructor — chạy khi gọi new Student(...)
    public Student(int id, string name, double gpa)
    {
        Id = id; Name = name; GPA = gpa;
    }

    // Method
    public void Print() =>
        Console.WriteLine($"[{Id}] {Name} | GPA: {GPA:F2}");
}

// Tạo object
Student sv = new Student(1, "Nam", 3.7);
sv.Print();
```

## List và LINQ cơ bản
```csharp
List<Student> list = new();
list.Add(new Student(1, "Nam", 3.7));

// Lọc
list.Where(s => s.GPA > 3.0).ToList()

// Sắp xếp
list.OrderByDescending(s => s.GPA).ToList()

// Tìm 1 phần tử (không crash nếu không có)
var sv = list.FirstOrDefault(s => s.Name == "Nam");

// Đếm, tổng, trung bình
list.Count(s => s.GPA > 3.5)
list.Sum(s => s.GPA)
list.Average(s => s.GPA)
list.Max(s => s.GPA)
```

---

# 🔷 PHẦN 2: OOP NÂNG CAO (Day 3)

## Kế thừa (Inheritance)
```csharp
class Animal                     // Class cha
{
    public string Name { get; set; }
    public virtual void Speak()  // virtual = con được phép ghi đè
        => Console.WriteLine("...");
}

class Dog : Animal               // Dog kế thừa Animal (dùng dấu :)
{
    public override void Speak() // override = ghi đè
        => Console.WriteLine("Gâu gâu!");
}

// Polymorphism — cùng gọi Speak() nhưng kết quả khác nhau
List<Animal> zoo = new() { new Dog(), new Cat() };
foreach (var a in zoo) a.Speak();
```

## Abstract Class
```csharp
abstract class Animal             // Không thể new Animal() trực tiếp
{
    public string Name { get; set; }
    public abstract void Speak(); // Bắt buộc class con override, không có {}
    public void Eat() => Console.WriteLine($"{Name} đang ăn"); // Method bình thường
}
```

## Interface
```csharp
interface IPayable               // I prefix = convention
{
    double CalculateSalary();    // Không có thân {}, không có access modifier
    void PrintPayslip();
}

class FullTimeEmployee : IPayable
{
    public double MonthlySalary { get; set; }
    public double CalculateSalary() => MonthlySalary; // Bắt buộc implement
    public void PrintPayslip() => Console.WriteLine($"Lương: {CalculateSalary():N0}");
}
```

| | Abstract Class | Interface |
|--|----------------|-----------|
| Có code? | ✅ Được | ❌ Không |
| Kế thừa | Chỉ 1 | Nhiều |
| Dùng khi | Code chung cần tái sử dụng | Chỉ định nghĩa "phải làm gì" |

---

# 🔷 PHẦN 3: COLLECTIONS + EXCEPTION + ASYNC (Day 4–5)

## Dictionary
```csharp
Dictionary<string, int> scores = new();
scores["Nam"] = 90;                          // Thêm/ghi đè
int s = scores["Nam"];                       // Đọc — crash nếu không có key!
scores.TryGetValue("Nam", out int val);      // Đọc AN TOÀN
scores.ContainsKey("Nam");                   // Kiểm tra key tồn tại

foreach (var kv in scores)
    Console.WriteLine($"{kv.Key}: {kv.Value}");
```

## GroupBy
```csharp
var groups = products.GroupBy(p => p.Category);
foreach (var group in groups)
{
    group.Key           // Tên nhóm: "Electronics"
    group.Count()       // Số phần tử
    group.Sum(p => p.Price) // Tổng trong nhóm
}
```

## Exception Handling
```csharp
try { /* code có thể lỗi */ }
catch (SpecificException ex) { /* bắt lỗi cụ thể trước */ }
catch (Exception ex) { /* bắt mọi lỗi còn lại */ }
finally { /* LUÔN chạy dù có lỗi hay không */ }

// Custom Exception
class NotFoundException : Exception
{
    public NotFoundException(int id)
        : base($"Không tìm thấy Id = {id}") { }
}
throw new NotFoundException(99);
```

## Async / Await
```csharp
// Hàm async phải trả về Task hoặc Task<T>
// Tên hàm có hậu tố Async (convention)
public static async Task<List<string>> FetchDataAsync()
{
    await Task.Delay(3000); // Giả lập chờ DB/API
    return new List<string> { "Item1", "Item2" };
}

// Main cũng phải là async nếu dùng await
static async Task Main()
{
    var data = await FetchDataAsync(); // Đợi không block thread
}
```

---

# 🔷 PHẦN 4: SQL SERVER (Day 6–7)

## Tạo Database và Table
```sql
CREATE DATABASE ShopDB;
GO
USE ShopDB;
GO

CREATE TABLE Categories (
    Id   INT IDENTITY(1,1) PRIMARY KEY,  -- Tự tăng
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    Name       NVARCHAR(200) NOT NULL,
    Price      DECIMAL(18,2) NOT NULL,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id)
    -- Phải tạo bảng cha (Categories) TRƯỚC
);
```

## CRUD
```sql
-- Thêm (N trước chuỗi để hỗ trợ tiếng Việt)
INSERT INTO Categories(Name) VALUES (N'Điện tử'), (N'Thời trang');

-- Đọc
SELECT * FROM Products;
SELECT * FROM Products WHERE Price > 100000 AND CategoryId = 1;

-- Sửa (BẮT BUỘC phải có WHERE)
UPDATE Products SET Price = 30000000 WHERE Id = 1;

-- Xóa (BẮT BUỘC phải có WHERE)
DELETE FROM Products WHERE Id = 5;
```

## JOIN và GROUP BY
```sql
-- INNER JOIN: Lấy sản phẩm kèm tên danh mục
SELECT p.Name, p.Price, c.Name AS CategoryName
FROM Products p
INNER JOIN Categories c ON p.CategoryId = c.Id;

-- GROUP BY: Đếm sản phẩm theo danh mục
SELECT CategoryId, COUNT(Id) AS Total, AVG(Price) AS AvgPrice
FROM Products
GROUP BY CategoryId
HAVING COUNT(Id) >= 2; -- Lọc nhóm (giống WHERE nhưng cho nhóm)
```

---

# 🔷 PHẦN 5: ENTITY FRAMEWORK CORE (Day 8–9)

## Cấu trúc cơ bản
```csharp
// 1. Entity = Class đại diện cho Table
public class Product
{
    public int Id { get; set; }       // Tự thành Primary Key
    public string Name { get; set; }
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } // Navigation Property
}

// 2. DbContext = Cầu nối
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}

// 3. Program.cs — đăng ký DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS; Database=MyDB; Trusted_Connection=True; TrustServerCertificate=True;")
);
```

## Migration Commands
```
Add-Migration InitDB    // Tạo "bản vẽ" từ code C#
Update-Database         // Thi công xuống SQL Server
```

## CRUD với EF Core
```csharp
using var db = new AppDbContext();

// CREATE
db.Products.Add(new Product { Name = "Laptop", Price = 25000000 });
db.SaveChanges(); // BẮT BUỘC — không có dòng này DB không thay đổi

// READ
var all    = db.Products.ToList();
var p      = db.Products.Find(1);                            // Tìm theo Id
var cheap  = db.Products.Where(p => p.Price < 5_000_000).ToList();
var joined = db.Products.Include(p => p.Category).ToList(); // Thay thế JOIN

// UPDATE
var p = db.Products.Find(1);
if (p != null) { p.Price = 30000000; db.SaveChanges(); }

// DELETE
var p = db.Products.Find(5);
if (p != null) { db.Products.Remove(p); db.SaveChanges(); }
```

---

# 🔷 PHẦN 6: ASP.NET CORE WEB API (Day 10)

## HTTP Methods
| Method | SQL tương đương | URL ví dụ |
|--------|-----------------|-----------|
| GET | SELECT | `/api/products` hoặc `/api/products/1` |
| POST | INSERT | `/api/products` |
| PUT | UPDATE | `/api/products/1` |
| DELETE | DELETE | `/api/products/1` |

## Program.cs tối thiểu
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("connection_string_here")
);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(); // Truy cập tại: https://localhost:{port}/swagger
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
```

## Controller cơ bản
```csharp
[ApiController]
[Route("api/[controller]")]          // URL: /api/products
public class ProductsController : ControllerBase  // BẮT BUỘC kế thừa
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _db.Products.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _db.Products.FindAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product p)
    {
        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product updated) { ... }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) { ... }
}
```

## Response Codes
| Hàm | Code | Dùng khi |
|-----|------|---------|
| `Ok(data)` | 200 | Thành công, có data |
| `CreatedAtAction(...)` | 201 | Tạo mới thành công |
| `NoContent()` | 204 | Thành công, không có data (DELETE) |
| `BadRequest(msg)` | 400 | Dữ liệu không hợp lệ |
| `Unauthorized()` | 401 | Chưa đăng nhập |
| `Forbidden()` | 403 | Không đủ quyền |
| `NotFound()` | 404 | Không tìm thấy |

---

# 🔷 PHẦN 7: LAYERED ARCHITECTURE (Day 11)

## Nguyên tắc tách tầng
```
Controller  → Nhận request, trả response, KHÔNG chứa logic
Service     → Business logic, validate, KHÔNG truy vấn DB trực tiếp
Repository  → Chỉ truy vấn Database
```

## Cấu trúc thư mục chuẩn
```
Models/
    Product.cs
Data/
    AppDbContext.cs
Repositories/
    IProductRepository.cs   ← Interface
    ProductRepository.cs    ← Implement
Services/
    IProductService.cs      ← Interface
    ProductService.cs       ← Implement
Controllers/
    ProductsController.cs
Program.cs
```

## Dependency Injection
```csharp
// Đăng ký trong Program.cs
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// ASP.NET Core tự inject qua Constructor
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    public ProductsController(IProductService service) { _service = service; }
}
```

---

# 🔷 PHẦN 8: DTO + VALIDATION (Day 12)

## Tại sao cần DTO?
- **CreateDto**: Client gửi lên — không có `Id` (tránh lỗi IDENTITY_INSERT)
- **ResponseDto**: Server trả về — kiểm soát field nào được lộ ra ngoài

## Data Annotations
```csharp
public class ProductCreateDto
{
    [Required(ErrorMessage = "Không được để trống")]
    [StringLength(200, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(1000, 999_000_000, ErrorMessage = "Giá từ 1,000 đến 999,000,000")]
    public double Price { get; set; }
}
```

## Kiểm tra Validation trong Controller
```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
{
    if (!ModelState.IsValid) return BadRequest(ModelState); // 400 + chi tiết lỗi
    // ...
}
```

## Mapper
```csharp
public static class ProductMapper
{
    public static Product ToEntity(ProductCreateDto dto) => new() {
        Name = dto.Name, Price = dto.Price
    };
    public static ProductResponseDto ToDto(Product p) => new() {
        Id = p.Id, Name = p.Name, Price = p.Price
    };
    public static List<ProductResponseDto> ToDtoList(List<Product> list)
        => list.Select(p => ToDto(p)).ToList();
}
```

---

# 🔷 PHẦN 9: JWT AUTHENTICATION (Day 13)

## Luồng hoạt động
```
1. POST /api/auth/register { username, password }
   → Hash password → Lưu vào bảng Users

2. POST /api/auth/login { username, password }
   → Kiểm tra → Tạo JWT Token → Trả về

3. GET /api/tasks (Header: Authorization: Bearer eyJ...)
   → Server verify token → Hợp lệ → Cho phép
```

## Cấu hình JWT trong Program.cs
```csharp
// appsettings.json
"JwtSettings": {
    "SecretKey": "MySuperSecretKey12345678901234567890",
    "Issuer": "MyApi",
    "Audience": "MyApiUsers",
    "ExpiryMinutes": 60
}

// Program.cs
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true, ValidateAudience = true,
            ValidateLifetime = true, ValidateIssuerSigningKey = true,
            ValidIssuer   = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    });
builder.Services.AddAuthorization();

// THỨ TỰ MIDDLEWARE BẮT BUỘC
app.UseAuthentication(); // Trước
app.UseAuthorization();  // Sau
app.MapControllers();
```

## Bảo vệ Endpoint
```csharp
[Authorize]                    // Phải đăng nhập
[Authorize(Roles = "Admin")]   // Phải là Admin
[AllowAnonymous]               // Ai cũng được
```

## Lấy thông tin User từ Token trong Controller
```csharp
using System.Security.Claims;

private int GetCurrentUserId()
{
    var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return int.Parse(claim!);
}
```

## Swagger hiển thị nút 🔒 Authorize
```csharp
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization", Type = SecuritySchemeType.Http,
        Scheme = "Bearer", BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {{ 
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        }, Array.Empty<string>()
    }});
});
```

---

# 🔷 PHẦN 10: LỖI HAY GẶP TỔNG HỢP

| Lỗi | Nguyên nhân | Fix |
|-----|-------------|-----|
| `IDENTITY_INSERT is OFF` | POST gửi lên có `"id": 1` | Xóa trường `id` hoặc để `"id": 0` |
| `AddSwaggerGen` not found | Thiếu NuGet `Swashbuckle.AspNetCore` | Cài version 6.x.x |
| `Ok()` `NotFound()` not found | Thiếu `: ControllerBase` | Thêm `: ControllerBase` |
| Browser 404 | Vào sai URL | Thêm `/swagger` vào cuối |
| `RegisterDto` not found | Class tên sai | Đổi tên class đúng với tên đang dùng |
| `async` thiếu `await` | Method sync nhưng khai báo async | Dùng `return Task.CompletedTask` |
| 401 Unauthorized | Không có token | Đăng nhập, dán token vào Authorize 🔒 |
| 403 Forbidden | Sai Role | Sửa Role trong DB, Login lại lấy Token mới |
| FK Constraint khi xóa | Xóa bảng cha khi còn con | Xóa con trước, rồi mới xóa cha |
| NuGet version không tương thích | .NET 9 nhưng cài EF Core 10 | Chọn đúng version 9.x.x |

---

# 🔷 PHẦN 11: THỨ TỰ TẠO MỘT PROJECT TỪ ĐẦU

Mỗi lần tạo project Web API mới, làm theo đúng thứ tự này:

```
1. Tạo Console/Web API project trong VS2022
2. Cài NuGet:
   - Microsoft.EntityFrameworkCore.SqlServer (9.x.x)
   - Microsoft.EntityFrameworkCore.Tools (9.x.x)
   - Microsoft.AspNetCore.Authentication.JwtBearer (9.x.x)
   - Swashbuckle.AspNetCore (6.x.x)

3. Tạo thư mục: Models / Data / Repositories / Services / Controllers / DTOs

4. Code theo thứ tự:
   Models → DbContext → Migration → Repository → Service → Controller

5. Đăng ký trong Program.cs:
   AddDbContext → AddScoped<IRepo, Repo> → AddScoped<IService, Service>
   AddAuthentication → AddAuthorization

6. Middleware trong Program.cs:
   UseSwagger → UseSwaggerUI → UseHttpsRedirection
   UseAuthentication → UseAuthorization → MapControllers → Run

7. Migration:
   Add-Migration InitDB
   Update-Database

8. Test trong Swagger:
   https://localhost:{port}/swagger
```

---

# 🔷 PHẦN 12: CÂU HỎI PHỎNG VẤN CỐT LÕI

| Câu hỏi | Trả lời ngắn |
|---------|-------------|
| OOP 4 tính chất? | Encapsulation, Inheritance, Polymorphism, Abstraction |
| Interface vs Abstract? | Interface: contract thuần, implement nhiều. Abstract: có code, kế thừa 1 |
| Dependency Injection là gì? | Inject dependency từ bên ngoài qua Constructor thay vì tự `new` bên trong |
| Repository Pattern? | Tách truy vấn DB ra riêng, Service không đụng DbContext trực tiếp |
| JWT hoạt động thế nào? | Login → Server tạo token (Header.Payload.Signature) → Client gửi kèm mỗi request |
| 401 vs 403? | 401: Chưa xác thực. 403: Đã xác thực nhưng không đủ quyền |
| `async/await` để làm gì? | Tránh block thread khi chờ DB/API → tăng throughput server |
| AddScoped vs Singleton? | Scoped: 1 instance/request. Singleton: 1 instance mãi mãi |
| DTO là gì? | Object trung gian kiểm soát dữ liệu vào/ra, không expose Entity thẳng |
| Migration là gì? | Cơ chế sync code C# (Entity) xuống cấu trúc DB thật |
