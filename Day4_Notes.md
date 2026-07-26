## DAY 4 – Collections, LINQ Nâng Cao, Exception Handling

### Dictionary — Lưu theo key → value
```csharp
Dictionary<string, int> scores = new();

// Thêm
scores["Nam"] = 90;           // gán trực tiếp
scores.Add("Lan", 85);        // hoặc dùng Add()

// Đọc
int s = scores["Nam"];        // 90 — crash nếu key không tồn tại!

// Đọc AN TOÀN
if (scores.TryGetValue("Unknown", out int val))
    Console.WriteLine(val);
else
    Console.WriteLine("Không tìm thấy"); // không crash

// Kiểm tra key tồn tại
bool exists = scores.ContainsKey("Lan"); // true

// Duyệt
foreach (var kv in scores)
    Console.WriteLine($"{kv.Key}: {kv.Value}");
//                      ↑ key      ↑ value
```

> **Khi nào dùng Dictionary?** Khi cần tìm nhanh theo key (tên, mã ID...) mà không muốn duyệt cả list.

---

### LINQ GroupBy — Nhóm dữ liệu
```csharp
var products = new List<Product> { ... };

// GroupBy nhóm theo Category
var groups = products.GroupBy(p => p.Category);

// groups = tập hợp các nhóm:
// - Nhóm "Electronics" → [Laptop, Mouse, Headset]
// - Nhóm "Furniture"   → [Desk, Chair]

foreach (var group in groups)
{
    string category = group.Key;              // tên nhóm: "Electronics"
    int count       = group.Count();          // số phần tử: 3
    double total    = group.Sum(p => p.Price);// tổng giá
    Console.WriteLine($"{category}: {count} SP | Tổng: {total:N0}");
}
```

---

### Exception Handling — Luồng đầy đủ

```
Code chạy bình thường
    │
    ├── Có lỗi xảy ra trong try { }
    │       │
    │       ├── Khớp catch (SpecificException) → chạy block đó
    │       ├── Khớp catch (Exception)         → chạy block đó
    │       └── Không khớp catch nào           → crash, lỗi nổi lên trên
    │
    └── finally { } LUÔN LUÔN chạy dù có lỗi hay không
```

```csharp
try
{
    var p = manager.GetById(99); // ném ProductNotFoundException
    Console.WriteLine(p.Name);   // dòng này KHÔNG chạy nếu GetById throws
}
catch (ProductNotFoundException ex)  // bắt đúng loại → vào đây
{
    Console.WriteLine($"❌ {ex.Message}");
}
catch (Exception ex)                 // bắt mọi lỗi còn lại
{
    Console.WriteLine($"Lỗi không xác định: {ex.Message}");
}
finally
{
    Console.WriteLine("Xong!");      // luôn chạy
}
```

---

### Custom Exception — Luồng chi tiết

```
Program.cs                  ProductManager.cs           ProductNotFoundException.cs
    │                               │                           │
    ├── manager.GetById(99) ──────→ │                           │
    │                               ├── FirstOrDefault → null   │
    │                               ├── null → throw new ───────┤
    │                               │   ProductNotFoundException(99)
    │                               │                           │
    │   ← Exception ────────────────┘   message = "Không tìm   │
    │                                    thấy sản phẩm Id = 99" │
    ├── catch (ProductNotFoundException ex)                      │
    │       ex.Message = "Không tìm thấy sản phẩm Id = 99"     │
    └── In ra màn hình
```

```csharp
// Định nghĩa
class ProductNotFoundException : Exception
{
    public ProductNotFoundException(int id)
        : base($"Không tìm thấy sản phẩm với Id = {id}")
    // ↑ : base() = gọi constructor của class cha (Exception)
    // ↑ truyền message → sau này dùng ex.Message sẽ có giá trị này
    { }
}

// Ném
throw new ProductNotFoundException(99);
// → tạo object exception với message đã định nghĩa, ném lên trên

// Bắt
catch (ProductNotFoundException ex)
{
    ex.Message  // = "Không tìm thấy sản phẩm với Id = 99"
}
```

---

### ProductManager — Luồng đầy đủ

```
Program.cs                     ProductManager.cs
    │
    ├── new ProductManager()
    │       → _products = List<Product> rỗng
    │
    ├── AddProduct(p) ────────→ _products.Add(p)
    │
    ├── GetById(99) ──────────→ FirstOrDefault(p => p.Id == 99)
    │                               → null → throw ProductNotFoundException
    │   ← catch exception ←────────┘
    │
    ├── GetTopExpensive(3) ───→ OrderByDescending(Price).Take(3).ToList()
    │   ← List<Product> (3 sp đắt nhất)
    │
    └── PrintSummaryByCategory()
            → GroupBy(Category)
            → foreach group:
                group.Key         = "Electronics"
                group.Count()     = 3
                group.Sum(Price)  = 27,000,000
```

---

### `ForEach` vs `foreach`
```csharp
// foreach — C# keyword, dùng với mọi IEnumerable
foreach (var p in list) p.Print();

// .ForEach() — method của List<T>, ngắn hơn
list.ForEach(p => p.Print());  // tương đương, chỉ dùng được với List<T>
```

---

### Tóm tắt LINQ đã học
```csharp
// Lọc + sắp xếp + lấy n phần tử
list.Where(p => p.Price > 1_000_000)
    .OrderByDescending(p => p.Price)
    .Take(3)
    .ToList()

// Nhóm
list.GroupBy(p => p.Category)

// Biến đổi
list.Select(p => p.Name)                    // lấy 1 field
list.Select(p => new { p.Name, p.Price })   // tạo object mới

// Tính toán
list.Sum(p => p.Price)
list.Average(p => p.Price)
list.Max(p => p.Price)
list.MaxBy(p => p.Price)    // trả cả object, không chỉ giá trị

// Kiểm tra
list.Any(p => p.Price > 10_000_000)
list.All(p => p.Price > 0)
```

---


