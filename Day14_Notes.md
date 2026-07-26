# 📚 DAY 14 – Mini Project: TaskApi (Tổng Kết)

---

### PROJECT HOÀN THÀNH: Task Management API

Kết hợp toàn bộ kiến thức 14 ngày vào 1 project thực tế.

---

### CẤU TRÚC PROJECT
```
TaskApi/
├── Models/
│   ├── User.cs         ← Bảng Users (Id, Username, PasswordHash, Role)
│   └── TaskItem.cs     ← Bảng Tasks (Id, Title, Description, IsCompleted, UserId)
├── Data/
│   └── AppDbContext.cs ← DbContext, DbSet<User>, DbSet<TaskItem>
├── DTOs/
│   ├── AuthDto.cs      ← RegisterDto, LoginDto, TokenResponseDto
│   └── TaskDto.cs      ← TaskCreateDto, TaskUpdateDto, TaskResponseDto
├── Services/
│   ├── AuthService.cs  ← Register, Login, GenerateToken, HashPassword
│   └── TaskService.cs  ← GetMyTasks, GetById, Create, Update, Delete
├── Controllers/
│   ├── AuthController.cs   ← POST /api/auth/register, POST /api/auth/login
│   └── TasksController.cs  ← GET/POST/PUT/DELETE /api/tasks
├── Program.cs
└── appsettings.json    ← JwtSettings: SecretKey, Issuer, Audience, ExpiryMinutes
```

---

### LUỒNG DỮ LIỆU ĐẦY ĐỦ
```
POST /api/auth/register
    → RegisterDto (validate) → AuthService.RegisterAsync()
    → HashPassword → Lưu User vào DB → "Đăng ký thành công"

POST /api/auth/login
    → LoginDto → AuthService.LoginAsync()
    → Kiểm tra password hash → GenerateToken()
    → Trả về JWT Token (Header.Payload.Signature)

POST /api/tasks  (Bearer Token)
    → [Authorize] kiểm tra token
    → GetCurrentUserId() đọc từ Claims
    → TaskCreateDto (validate) → TaskService.CreateAsync()
    → Lưu TaskItem với UserId → Trả về TaskResponseDto

GET /api/tasks/1  (Bearer Token của User khác)
    → GetByIdAsync(id, userId) — tìm với cả 2 điều kiện
    → Không tìm thấy → 404 (không để lộ task người khác tồn tại)
```

---

### BÀI HỌC QUAN TRỌNG NHẤT TRONG PROJECT NÀY

**1. Bảo mật dữ liệu theo UserId:**
```csharp
// ❌ SAI — lấy tất cả task của mọi người
var tasks = await _db.Tasks.ToListAsync();

// ✅ ĐÚNG — chỉ lấy task của user đang đăng nhập
var tasks = await _db.Tasks
    .Where(t => t.UserId == userId)
    .ToListAsync();
```

**2. Trả 404 thay vì 403 khi user cố xem task người khác:**
```csharp
// Không để lộ thông tin task của người khác tồn tại
var task = await _db.Tasks
    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
if (task == null) return NotFound(); // 404 thay vì 403
```

**3. Thứ tự đăng ký Services trong Program.cs:**
```csharp
// TẤT CẢ AddScoped phải TRƯỚC builder.Build()
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build(); // ← Ranh giới quan trọng!

// Middleware SAU Build()
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

### LỖI GẶP TRONG PROJECT NÀY
| Lỗi | Nguyên nhân | Fix |
|-----|-------------|-----|
| `Service collection is read-only` | AddScoped đặt sau `builder.Build()` | Chuyển lên trước `Build()` |
