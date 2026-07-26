using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ProductWebApi.Data;
using ProductWebApi.DTOs;
using ProductWebApi.Models;

namespace ProductWebApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        // Đăng ký tài khoản mới
        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            // Kiểm tra username đã tồn tại chưa
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return null; // null = đã tồn tại
            // Mã hóa mật khẩu trước khi lưu (KHÔNG BAO GIỜ lưu mật khẩu thô!)
            string hash = BCryptHash(dto.Password);
            var user = new User { Username = dto.Username, PasswordHash = hash };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return "Đăng ký thành công!";
        }
        // Đăng nhập và tạo JWT Token
        public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            // Kiểm tra user tồn tại và mật khẩu đúng không
            if (user == null || !VerifyHash(dto.Password, user.PasswordHash))
                return null; // null = sai username hoặc password
            return GenerateToken(user);
        }
        // Tạo JWT Token
        private TokenResponseDto GenerateToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]!;
            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]!);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Claims = thông tin được nhúng vào trong Token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var expiry = DateTime.UtcNow.AddMinutes(expiryMinutes);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );
            return new TokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = user.Username,
                ExpiresAt = expiry
            };
        }
        // Mã hóa mật khẩu bằng SHA256 (đơn giản để học)
        private string BCryptHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
        private bool VerifyHash(string password, string hash)
            => BCryptHash(password) == hash;
    
    }
}
