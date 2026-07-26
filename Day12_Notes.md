# 📚 DAY 12 – DTO + Data Validation

---

### 1. TẠI SAO CẦN DTO?

| Vấn đề khi dùng Entity trực tiếp | Giải pháp với DTO |
|-----------------------------------|-------------------|
| Client tự đặt được `Id` khi POST → lỗi IDENTITY_INSERT | `ProductCreateDto` không có trường `Id` |
| Server trả về field nhạy cảm (password, internal data) | `ProductResponseDto` chỉ chứa field được phép |
| Không validate dữ liệu đầu vào | Dùng Data Annotations trên DTO |

---

### 2. CÁC DTO CẦN TẠO

```
ProductCreateDto   ← Client GỬI LÊN (POST, PUT) — không có Id
ProductResponseDto ← Server TRẢ VỀ (GET) — có Id, có thể có field bổ sung
ProductMapper      ← Chuyển đổi giữa DTO ↔ Entity
```

---

### 3. DATA ANNOTATIONS (Validate tự động)

```csharp
using System.ComponentModel.DataAnnotations;

public class ProductCreateDto
{
    [Required(ErrorMessage = "Không được để trống")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Phải từ 2-200 ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1000, 999_000_000, ErrorMessage = "Giá phải từ 1,000 đến 999,000,000")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Không được để trống")]
    public string Category { get; set; } = string.Empty;
}
```

**Các Annotation thường dùng:**
| Annotation | Tác dụng |
|-----------|---------|
| `[Required]` | Không được null/empty |
| `[StringLength(max, MinimumLength = min)]` | Giới hạn độ dài chuỗi |
| `[Range(min, max)]` | Giới hạn khoảng số |
| `[EmailAddress]` | Phải đúng định dạng email |
| `[MinLength(n)]` | Tối thiểu n ký tự |
| `[MaxLength(n)]` | Tối đa n ký tự |

---

### 4. PRODUCTMAPPER — Chuyển đổi DTO ↔ Entity

```csharp
public static class ProductMapper
{
    // DTO → Entity (nhận từ client, lưu vào DB)
    public static Product ToEntity(ProductCreateDto dto) => new Product
    {
        Name = dto.Name, Price = dto.Price, Category = dto.Category
    };

    // Entity → ResponseDto (lấy từ DB, trả về client)
    public static ProductResponseDto ToDto(Product p) => new ProductResponseDto
    {
        Id = p.Id, Name = p.Name, Price = p.Price, Category = p.Category
    };

    // Danh sách Entity → Danh sách Dto
    public static List<ProductResponseDto> ToDtoList(List<Product> products)
        => products.Select(p => ToDto(p)).ToList();
}
```

---

### 5. KIỂM TRA VALIDATION TRONG CONTROLLER

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
{
    // ASP.NET Core tự kiểm tra các Annotation trên DTO
    // Nếu sai → ModelState.IsValid = false
    if (!ModelState.IsValid)
        return BadRequest(ModelState); // 400 + danh sách lỗi chi tiết

    var entity = ProductMapper.ToEntity(dto); // DTO → Entity
    var created = await _service.CreateAsync(entity);
    return CreatedAtAction(nameof(GetById), new { id = created.Id }, ProductMapper.ToDto(created));
}
```

---

### 6. LUỒNG DỮ LIỆU ĐẦY ĐỦ

```
Client POST JSON
    │
    ▼  [Required], [Range]... tự động kiểm tra
ProductCreateDto ──(validate fail)──→ 400 Bad Request
    │ (validate pass)
    ▼  ProductMapper.ToEntity()
Product Entity
    │
    ▼  _service.CreateAsync()
SQL Server (lưu vào DB, cấp phát Id)
    │
    ▼  ProductMapper.ToDto()
ProductResponseDto ──────────────→ 201 Created (trả về JSON)
```
