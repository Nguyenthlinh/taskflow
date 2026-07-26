using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Models;

namespace TaskApi.Services
{
    //Service chứa toàn bộ logic nghiệp vụ.
    //Controller chỉ gọi Service, không tự xử lý gì cả.
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db; _config = config;
        }
        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            // Kiểm tra username đã tồn tại chưa
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return null; // null = đã tồn tại
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password)
                // KHÔNG BAO GIỜ lưu mật khẩu thô vào DB!
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return "Đăng ký thành công!";
        }
        public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                return null; // Sai username hoặc password
            return GenerateToken(user);
        }
        private TokenResponseDto GenerateToken(User user)
        {
            var jwt = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpiryMinutes"]!));
            // Claims = thông tin nhúng vào trong Token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // UserId
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"], audience: jwt["Audience"],
                claims: claims, expires: expiry, signingCredentials: creds
            );
            return new TokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.Username,
                ExpiresAt = expiry
            };
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        private bool VerifyPassword(string password, string hash)
            => HashPassword(password) == hash;
    }
}
