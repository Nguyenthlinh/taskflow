using Microsoft.AspNetCore.Mvc;
using ProductWebApi.DTOs;
using ProductWebApi.Services;

namespace ProductWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
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