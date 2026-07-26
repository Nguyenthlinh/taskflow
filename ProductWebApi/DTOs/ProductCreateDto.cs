using System.ComponentModel.DataAnnotations;

namespace ProductWebApi.DTOs
{
    // DTO dùng khi CLIENT GỬI LÊN (POST, PUT)
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Tên phải từ 2 đến 200 ký tự")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1000, 999_000_000, ErrorMessage = "Giá phải từ 1,000 đến 999,000,000")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Danh mục không được để trống")]
        public string Category { get; set; } = string.Empty;
    }
}
