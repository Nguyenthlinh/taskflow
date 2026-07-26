# 📚 NOTES – Backend .NET Learning

---

## ⚡ BẢNG ÔN NHANH – TOÀN BỘ LÝ THUYẾT

> Đọc bảng này trước mỗi buổi học để nhớ lại kiến thức cũ.

### 🔷 C# Syntax

| Concept | Cú pháp | Ghi nhớ |
|---------|---------|---------|
| String interpolation | `$"Tên: {name}"` | Nhúng biến vào chuỗi |
| Format số | `{price:N0}` `{gpa:F2}` | N0=phân cách nghìn, F2=2 thập phân |
| Null coalescing | `x ?? "default"` | Nếu x null thì dùng "default" |
| Null conditional | `obj?.Name` | Nếu obj null thì trả null, không crash |
| Parse an toàn | `double.TryParse(s, out double d)` | Không crash nếu nhập sai |
| Loop vô hạn | `while(true) { ... break; }` | Dùng khi không biết số lần lặp |
| Số dễ đọc | `15_000_000` | `_` không ảnh hưởng giá trị |
| Kiểu nullable | `int? x = null;` | Biến có thể chứa null |

---

### 🔷 OOP – Class & Object

| Concept | Cú pháp | Ghi nhớ |
|---------|---------|---------|
| Khai báo class | `class Student { }` | Blueprint/bản thiết kế |
| Tạo object | `new Student(1, "Nam", 3.7)` | Gọi constructor |
| Property | `public string Name { get; set; }` | Data của class |
| Read-only property | `public string Name { get; private set; }` | Ngoài class chỉ đọc |
| Constructor | `public Student(int id, string name) { }` | Chạy khi new() |
| Private field | `private List<T> _items = new();` | `_` prefix = private field |
| Public method | `public void DoSomething() { }` | Cho phép gọi từ ngoài |
| `this` | `this.name = name;` | Phân biệt field vs tham số cùng tên |

---

### 🔷 OOP – Inheritance, Abstract, Interface

| Concept | Cú pháp | Ghi nhớ |
|---------|---------|---------|
| Kế thừa | `class Dog : Animal` | Dog có tất cả của Animal |
| Ghi đè | `virtual` → `override` | virtual = cho phép, override = ghi đè |
| Abstract class | `abstract class Animal` | Không new() trực tiếp được |
| Abstract method | `public abstract void Speak();` | Bắt buộc class con override, không có `{}` |
| Interface | `interface IPayable { }` | Chỉ contract, không có code |
| Implement interface | `class X : IPayable` | Bắt buộc implement tất cả methods |
| Polymorphism | `List<Animal> { new Dog(), new Cat() }` | Cùng gọi `Speak()` → kết quả khác nhau |
| I prefix | `IPayable`, `ILogger` | Convention đặt tên interface |

**Abstract vs Interface:**
| | Abstract Class | Interface |
|--|----------------|-----------|
| Có code? | ✅ Được | ❌ Không |
| Kế thừa | Chỉ 1 | Nhiều |
| Dùng khi | Code chung cần tái sử dụng | Chỉ định nghĩa "phải làm gì" |

---

### 🔷 Collections

| Collection | Khai báo | Dùng khi |
|-----------|---------|---------|
| `List<T>` | `List<string> names = new();` | Duyệt tuần tự, thứ tự quan trọng |
| `Dictionary<K,V>` | `Dictionary<string, int> d = new();` | Tìm nhanh theo key |

| Dictionary Method | Làm gì |
|------------------|--------|
| `d["key"] = val` | Thêm/ghi đè |
| `d["key"]` | Đọc — crash nếu key không tồn tại |
| `d.TryGetValue("key", out val)` | Đọc AN TOÀN |
| `d.ContainsKey("key")` | Kiểm tra key tồn tại |
| `kv.Key` / `kv.Value` | Dùng khi foreach Dictionary |

---

### 🔷 LINQ

| Method | Làm gì | Ví dụ |
|--------|--------|-------|
| `Where` | Lọc | `.Where(p => p.Price > 1000)` |
| `OrderBy` | Sắp xếp tăng | `.OrderBy(p => p.Price)` |
| `OrderByDescending` | Sắp xếp giảm | `.OrderByDescending(p => p.Price)` |
| `Take(n)` | Lấy n phần tử đầu | `.Take(3)` |
| `Select` | Biến đổi | `.Select(p => p.Name)` |
| `GroupBy` | Nhóm theo field | `.GroupBy(p => p.Category)` |
| `FirstOrDefault` | Tìm 1, null nếu không có | `.FirstOrDefault(p => p.Id == 1)` |
| `Count` | Đếm | `.Count(p => p.Price > 0)` |
| `Sum` | Tổng | `.Sum(p => p.Price)` |
| `Average` | Trung bình | `.Average(p => p.Price)` |
| `Max/Min` | Giá trị lớn nhất/nhỏ nhất | `.Max(p => p.Price)` |
| `MaxBy/MinBy` | Object có giá trị lớn nhất | `.MaxBy(p => p.Price)` |
| `Any` | Có phần tử thỏa không? | `.Any(p => p.Price > 10000)` |
| `All` | Tất cả thỏa không? | `.All(p => p.Price > 0)` |
| `ToList()` | Thực thi query, ra List | Luôn gọi cuối chain |
| `ForEach` | Duyệt List và làm gì đó | `list.ForEach(p => p.Print())` |

**Lambda Expression:**
```
p => p.Price > 1000
↑      ↑
tên    điều kiện/biểu thức
tạm    (đặt gì cũng được: p, x, item...)
```

---

### 🔷 Exception Handling

| Keyword | Làm gì |
|---------|--------|
| `try { }` | Bọc code có thể lỗi |
| `catch (Exception ex)` | Bắt lỗi, xử lý |
| `finally { }` | Luôn chạy dù có lỗi hay không |
| `throw` | Ném exception |
| `ex.Message` | Chuỗi mô tả lỗi |
| `ex.StackTrace` | Nơi lỗi xảy ra (dùng khi debug) |

**Custom Exception:**
```csharp
class NotFoundException : Exception
{
    public NotFoundException(int id)
        : base($"Không tìm thấy Id = {id}") { }
    //  ↑ : base() = gọi constructor của Exception, set Message
}
// Dùng: throw new NotFoundException(99);
// Bắt:  catch (NotFoundException ex) { ex.Message }
```

**Thứ tự catch — từ cụ thể → chung:**
```csharp
catch (ProductNotFoundException ex) { } // ✅ cụ thể trước
catch (Exception ex)                { } // ✅ chung sau
// ❌ Đừng đảo ngược — Exception bắt hết, không xuống được
```

---

### 🔷 Lỗi Hay Gặp

| Lỗi | Nguyên nhân | Fix |
|-----|-------------|-----|
| `NullReferenceException` | Dùng object null | Check `if (x != null)` hoặc dùng `?.` |
| `FormatException` | Parse sai kiểu | Dùng `TryParse` thay `Parse` |
| `KeyNotFoundException` | Đọc Dictionary key không tồn tại | Dùng `TryGetValue` |
| `{Method}` in ra tên method | Thiếu `()` | Sửa thành `{Method()}` |
| Interface không compile | Tạo `class` thay vì `interface` | Đổi `class` → `interface` |
| Code ngoài `Main()` | Code thực thi không trong method | Chuyển vào trong `Main` |
| App đóng ngay | Thiếu `Console.ReadLine()` cuối | Thêm vào cuối `Main` |

---


