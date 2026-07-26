using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
namespace TaskApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;// Mặc định chưa hoàn thành
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;//Tự động lấy thời gian hiện tại khi tạo TaskItem 

        //Khóa ngoại để liên kết với User
        public int UserId { get; set; }
        public User User { get; set; } = null!; //gọi là Navigation Property — EF Core dùng nó để biết Task có quan hệ với User.Giống như Foreign Key nhưng viết bằng C#.
    }
}
