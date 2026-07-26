using System.ComponentModel.DataAnnotations;
namespace TaskApi.DTOs
{
    //DTO = Data Transfer Object.Dùng để kiểm soát dữ liệu vào/ra.
    //    Không bao giờ dùng Entity thẳng cho request/response.
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username không được để trống")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username từ 3-50 ký tự")]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; } = string.Empty;
    }
    public class LoginDto
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }
    public class TokenResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
