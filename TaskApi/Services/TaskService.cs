using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Models;


namespace TaskApi.Services
{
    public class TaskService
    {
        private readonly AppDbContext _db;
        public TaskService(AppDbContext db) { _db = db; }
        
        // Lấy task CỦA ĐÚNG USER đang đăng nhập (quan trọng!)
        public async Task<List<TaskItem>> GetMyTasksAsync(int userId)
            => await _db.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        // Tìm theo Id VÀ UserId — tránh User A xem task của User B
        public async Task<TaskItem?> GetByIdAsync(int id, int userId)
            => await _db.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }
        public async Task<TaskItem?> UpdateAsync(int id, int userId, TaskUpdateDto dto)
        {
            var task = await GetByIdAsync(id, userId);
            if (task == null) return null;
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            await _db.SaveChangesAsync();
            return task;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var task = await GetByIdAsync(id, userId);
            if (task == null) return false;
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
