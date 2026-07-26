using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.DTOs;
using ProductWebApi.Models;
using ProductWebApi.Services;
using Microsoft.AspNetCore.Authorization;



namespace ProductWebApi.Controllers
{
    // Chỉ cho phép người đã đăng nhập gọi toàn bộ controller này
    [Authorize]
    [ApiController]
    [Route("api/[controller]")] // URL sẽ là: /api/products
    public class ProductsController : ControllerBase
    {
        // Controller chỉ gọi Service, không đụng DB trực tiếp nữa
        private readonly IProductService _service;
        public ProductsController(IProductService service)
        {
            _service = service;
        }
        // Hoặc cho phép tất cả xem, nhưng chỉ Admin mới xóa được
        [HttpGet]
        [AllowAnonymous] // Override [Authorize] — ai cũng xem được
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            // Chuyển sang DTO trước khi trả về — Client không thấy field nội bộ
            return Ok(ProductMapper.ToDtoList(products));
        }
        // GET /api/products/1 → Lấy 1 sản phẩm theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound($"Không tìm thấy Id = {id}");
            return Ok(ProductMapper.ToDto(product));
        }
        // POST /api/products → Thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            // ModelState.IsValid tự động kiểm tra các [Required], [Range]... trong DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Trả về danh sách lỗi validate
            try
            {
                var entity = ProductMapper.ToEntity(dto); // DTO → Entity
                var created = await _service.CreateAsync(entity);
                var result = ProductMapper.ToDto(created); // Entity → DTO để trả về
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // PUT /api/products/1 → Sửa sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = ProductMapper.ToEntity(dto);
            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null)
                return NotFound();
            return Ok(ProductMapper.ToDto(updated));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ role Admin mới xóa được
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        
    }
}
