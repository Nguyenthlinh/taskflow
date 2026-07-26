namespace ProductWebApi.DTOs
{
    // DTO dùng khi SERVER TRẢ VỀ (GET)
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string PriceFormatted => $"{Price:N0} VNĐ"; // Thêm field tiện lợi cho client
    }
}
