using System.ComponentModel.DataAnnotations;

namespace TaskApi.DTOs
{
    // Client GỬI LÊN khi tạo task mới — không có Id
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Tiêu đề từ 3-200 ký tự")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    // Client GỬI LÊN khi cập nhật task
    public class TaskUpdateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

    }
    // Server TRẢ VỀ cho client
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        // Computed property — tự tính dựa trên IsCompleted
        // Không lưu vào DB, chỉ hiện khi trả về cho client
        public string Status => IsCompleted ? "✅ Hoàn thành" : "⏳ Đang làm";
        public DateTime CreatedAt { get; set; }
    }
}
