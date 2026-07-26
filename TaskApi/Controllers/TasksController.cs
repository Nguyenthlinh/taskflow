using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskApi.DTOs;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Controllers
{
    [Authorize]                  // Toàn bộ controller yêu cầu đăng nhập
    [ApiController]
    [Route("api/[controller]")]  // URL: /api/tasks
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;
        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }
        // Helper: Đọc UserId từ JWT Token (không cần query DB)
        // ClaimTypes.NameIdentifier chính là user.Id.ToString() lúc tạo token
        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim!);
        }
        // Chuyển Entity → ResponseDto (dùng ở nhiều chỗ nên để riêng)
        private static TaskResponseDto ToDto(TaskItem t) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreatedAt
        };
        // GET /api/tasks → Lấy danh sách task CỦA MÌNH
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = GetCurrentUserId();
            var tasks = await _taskService.GetMyTasksAsync(userId);
            return Ok(tasks.Select(t => ToDto(t)).ToList());
        }
        // GET /api/tasks/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            int userId = GetCurrentUserId();
            var task = await _taskService.GetByIdAsync(id, userId);
            // Trả 404 thay vì 403 — không để lộ task của người khác tồn tại
            if (task == null) return NotFound("Không tìm thấy task!");
            return Ok(ToDto(task));
        }
        // POST /api/tasks → Tạo task mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int userId = GetCurrentUserId();
            var newTask = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = userId  // Gắn task vào đúng user đang đăng nhập
            };
            var created = await _taskService.CreateAsync(newTask);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, ToDto(created));
        }
        // PUT /api/tasks/1 → Cập nhật task
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int userId = GetCurrentUserId();
            var result = await _taskService.UpdateAsync(id, userId, dto);
            if (result == null) return NotFound("Không tìm thấy task!");
            return Ok(ToDto(result));
        }
        // DELETE /api/tasks/1 → Xóa task
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = GetCurrentUserId();
            bool ok = await _taskService.DeleteAsync(id, userId);
            if (!ok) return NotFound("Không tìm thấy task!");
            return NoContent(); // 204 — Xóa thành công
        }


    }
}
