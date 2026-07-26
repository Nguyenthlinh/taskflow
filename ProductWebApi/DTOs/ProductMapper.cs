using Azure.Core;
using ProductWebApi.Models;

namespace ProductWebApi.DTOs
{
    public class ProductMapper
    {
        // Chuyển DTO → Entity(khi tạo mới từ request của client)
        public static Product ToEntity(ProductCreateDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = dto.Category
            };
        }
        // Chuyển Entity → ResponseDto (khi trả về cho client)
        public static ProductResponseDto ToDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category
            };
        }
        // Chuyển danh sách Entity → danh sách Dto
        public static List<ProductResponseDto> ToDtoList(List<Product> products)
        {
            return products.Select(p => ToDto(p)).ToList();
        }
    }
}
