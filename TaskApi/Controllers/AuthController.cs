using Azure;
using Microsoft.AspNetCore.Mvc;
using TaskApi.DTOs;
using TaskApi.Services;
using static System.Net.WebRequestMethods;

namespace TaskApi.Controllers
{
    //Controller nhận HTTP Request → gọi Service → trả Response.
    //Không chứa logic, không đụng DB trực tiếp.
    [ApiController]
    [Route("api/[controller]")]  // URL: /api/auth

    public class AuthController: ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
      
        }
        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(dto);
            if (result == null)
                return Conflict("Username đã tồn tại!"); // 409
            return Ok(result);
        }
        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Sai username hoặc mật khẩu!"); // 401
            return Ok(token);
        }
    }


}
