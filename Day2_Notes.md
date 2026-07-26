## DAY 2 – OOP: Class, Object, Constructor

### Class vs Object
```csharp
// Class = bản thiết kế (blueprint)
class Student { ... }

// Object = sản phẩm tạo từ bản thiết kế
Student sv = new Student(1, "Nam", 20, 3.7);
//           ↑ gọi constructor
```

---

### Anatomy của Class
```csharp
public class Student
{
    // PROPERTIES = data
    public int Id { get; set; }
    public string Name { get; set; }
    public double GPA { get; set; }

    // CONSTRUCTOR = chạy khi new Student(...)
    public Student(int id, string name, double gpa)
    {
        Id = id;
        Name = name;
        GPA = gpa;
    }

    // METHOD = hành động
    public void PrintInfo()
    {
        Console.WriteLine($"[{Id}] {Name} | GPA: {GPA}");
    }
}
```

---

### `{ get; set; }` — Auto Property
```csharp
public string Name { get; set; }          // đọc + ghi tự do
public string Name { get; private set; }  // ngoài class chỉ đọc
public string Name { get; }               // readonly, chỉ gán trong constructor
```

---

### `public` vs `private`
```csharp
public class Classroom
{
    private List<Student> _students = new(); // ← private
    // Tại sao private?
    // → Không cho code ngoài tự ý Add/Remove/Clear list
    // → Bắt buộc đi qua method AddStudent() để kiểm soát
}

// ❌ Không muốn:
lop._students.Clear(); // bypass hết validate

// ✅ Muốn:
lop.AddStudent(sv);    // có thể validate, log trong method
```

> **Quy tắc**: Data → `private`. Method public mới được dùng từ ngoài.

---

### `_` prefix — Naming Convention
```csharp
private List<Student> _students = new();
// _ trước tên = quy ước ngầm: đây là private field
// 99% codebase .NET đều dùng convention này
```

---

### Constructor — Tại sao cần?
```csharp
// Không có constructor → có thể tạo object thiếu data → bug âm thầm
Student sv = new Student();
// sv.Name = null, sv.GPA = 0 → dùng sau bị lỗi

// Có constructor → bắt buộc truyền đủ, không thiếu được
Student sv = new Student(1, "Nam", 20, 3.7); // ✅
```

---

### LINQ đầy đủ
```csharp
var list = new List<Student> { ... };

// LỌC
list.Where(sv => sv.GPA > 3.0)

// TÌM 1 PHẦN TỬ — trả null nếu không có (không crash)
list.FirstOrDefault(sv => sv.Name == "Nam")

// SẮP XẾP
list.OrderBy(sv => sv.GPA)             // tăng dần
list.OrderByDescending(sv => sv.GPA)   // giảm dần

// LẤY N PHẦN TỬ ĐẦU
list.Take(3)

// ĐẾM
list.Count(sv => sv.GPA > 3.0)

// BIẾN ĐỔI — lấy ra 1 field
list.Select(sv => sv.Name)  // → danh sách tên

// KIỂM TRA
list.Any(sv => sv.GPA > 3.9)  // có ai không?
list.All(sv => sv.Age >= 18)  // tất cả đều không?

// GIÁ TRỊ
list.Max(sv => sv.GPA)
list.Min(sv => sv.GPA)
list.Average(sv => sv.GPA)
```

---

### Lambda Expression `=>`
```csharp
sv => sv.GPA > 3.0
// sv  = tên tạm cho từng phần tử (đặt gì cũng được: s, x, item...)
// =>  = "thì"
// sv.GPA > 3.0 = điều kiện / biểu thức

// Đọc thành câu: "với mỗi sv, thì sv.GPA > 3.0"
```

---

### `.ToList()` — Khi nào cần?
```csharp
// LINQ không chạy ngay, chỉ "ghi nhớ" truy vấn
var query = list.Where(sv => sv.GPA > 3.0);  // chưa chạy
var result = query.ToList();                   // chạy tại đây!

// Nếu chỉ đếm / kiểm tra → KHÔNG cần ToList
int count = list.Count(sv => sv.GPA > 3.0);  // không cần ToList
bool hasAny = list.Any(sv => sv.GPA > 3.9);  // không cần ToList
```

---

### `Student?` — Nullable Reference Type
```csharp
public Student? FindByName(string name) { ... }
// Dấu ? = method này có thể trả về null
// → Người dùng biết phải check null trước khi dùng

Student? sv = lop.FindByName("Nam");
if (sv != null)
    sv.PrintInfo();  // ✅ an toàn
```

---

## QUICK REFERENCE

| Concept | Dùng khi nào |
|---------|-------------|
| `double.TryParse()` | Đọc số từ input user, tránh crash |
| `?? "default"` | Thay thế giá trị null |
| `$"{var:F2}"` | Format số trong string |
| `while(true)` + `break` | Loop không biết trước số lần |
| `private` field | Data nội bộ của class |
| `public` method | Hành động cho phép bên ngoài gọi |
| Constructor | Bắt buộc đủ data khi tạo object |
| LINQ `.Where()` | Lọc danh sách |
| LINQ `.FirstOrDefault()` | Tìm 1 phần tử, không crash nếu null |
| Lambda `x => x.Field` | Biểu thức ngắn gọn trong LINQ |
| `abstract class` | Class cha có code chung, không new() trực tiếp |
| `interface` | Chỉ định nghĩa contract, không có code |
| `virtual` / `override` | Cho phép / ghi đè hành vi class cha |
| `I` prefix | Convention đặt tên interface: `IPayable`, `ILogger` |

---


