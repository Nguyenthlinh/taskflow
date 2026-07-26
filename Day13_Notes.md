# 📚 DAY 13 – JWT Authentication

---

### 1. LUỒNG HOẠT ĐỘNG

```
POST /api/auth/register { username, password }
    → Hash password → Lưu vào bảng Users

POST /api/auth/login { username, password }
    → Kiểm tra password → Tạo JWT Token → Trả về token

GET /api/products  (Header: Authorization: Bearer eyJ...)
    → Server giải mã Token → Hợp lệ → Cho phép truy cập
    → Không có Token → 401 Unauthorized
    → Có Token nhưng sai Role → 403 Forbidden
```

---

### 2. CÁC THÀNH PHẦN CẦN TẠO

| File | Vai trò |
|------|---------|
| `Models/User.cs` | Entity lưu tài khoản (Id, Username, PasswordHash, Role) |
| `DTOs/AuthDto.cs` | `RegisterDto`, `LoginDto`, `TokenResponseDto` |
| `Services/AuthService.cs` | Logic đăng ký, đăng nhập, tạo Token |
| `Controllers/AuthController.cs` | Endpoint POST /register và POST /login |

---

### 3. CẤU HÌNH JWT TRONG PROGRAM.CS

```csharp
// appsettings.json
"JwtSettings": {
    "SecretKey": "MySuperSecretKey12345678901234567890",
    "Issuer": "ProductWebApi",
    "Audience": "ProductWebApiUsers",
    "ExpiryMinutes": 60
}

// Program.cs
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,   // Token hết hạn → từ chối
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSettings["Issuer"],
            ValidAudience            = jwtSettings["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    });

// THỨ TỰ MIDDLEWARE BẮT BUỘC:
app.UseAuthentication(); // Phải TRƯỚC
app.UseAuthorization();  // Phải SAU
app.MapControllers();
```

---

### 4. BẢO VỆ ENDPOINT

```csharp
[Authorize]                    // Phải đăng nhập (mọi role)
[Authorize(Roles = "Admin")]   // Chỉ Admin
[AllowAnonymous]               // Ai cũng được (override [Authorize])
```

---

### 5. CẤU HÌNH SWAGGER HIỂN THỊ NÚT 🔒 AUTHORIZE

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http,
        Scheme = "Bearer", BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
```

---

### 6. 401 vs 403 — Phân biệt

| Code | Tên | Nguyên nhân | Fix |
|------|-----|-------------|-----|
| **401** | Unauthorized | Không có token / Token sai | Đăng nhập lại |
| **403** | Forbidden | Có token nhưng sai Role | Cấp đúng Role cho user |

---

### 7. LỖI HAY GẶP

| Lỗi | Nguyên nhân | Fix |
|-----|-------------|-----|
| Nút 🔒 Authorize không hiện | Chưa cấu hình `AddSecurityDefinition` | Thêm vào `AddSwaggerGen()` |
| 403 khi DELETE | User có Role="User", endpoint yêu cầu "Admin" | Sửa Role trong DB, Login lại lấy Token mới |
| `RegisterDto` not found | Tạo file tên `AuthDto.cs` nhưng class tên `AuthDto` | Đổi tên class thành `RegisterDto` |
| `async` method thiếu `await` | `Update()` và `Remove()` là sync | Dùng `Task.CompletedTask` |
