using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskApi.Data;
using TaskApi.Services;



var builder = WebApplication.CreateBuilder(args);

// 1. Controllers
builder.Services.AddControllers();

// 2. Swagger (có hỗ trợ nút 🔒 Authorize)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {{
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer"
            }
        }, Array.Empty<string>()
    }});
});
// 3. DbContext — nhớ đổi "SQLEXPRESS" thành tên server của bạn nếu khác
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=.\\HOAILINH; Database=TaskApiDB; Trusted_Connection=True; TrustServerCertificate=True;")

);
// 4. JWT Authentication
var jwt = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["SecretKey"]!))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();

// 5. CORS — Cho phép trang HTML gọi API (không có cái này browser sẽ chặn)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép mọi nguồn (file://, localhost:xxxx...)
              .AllowAnyHeader()  // Cho phép mọi header (Authorization, Content-Type...)
              .AllowAnyMethod(); // Cho phép GET, POST, PUT, DELETE...
    });
});
// ===== BUILD =====
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // ← CORS phải đứng TRƯỚC Authentication
app.UseAuthentication();
app.UseAuthorization();
// AddScoped = tạo 1 instance mới cho mỗi HTTP Request.
// ASP.NET Core sẽ tự động inject vào Constructor của Controller.

app.MapControllers();
app.Run();
