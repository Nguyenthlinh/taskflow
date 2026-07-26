## DAY 1 – C# Syntax Cơ Bản

### `double.Parse()` vs `double.TryParse()`
```csharp
// Parse → crash nếu nhập chữ vào ô số
double diem = double.Parse("abc"); // ❌ FormatException

// TryParse → an toàn hơn, trả false thay vì crash
if (double.TryParse("abc", out double diem))
    Console.WriteLine(diem);
else
    Console.WriteLine("Nhập sai!"); // ✅
```

---

### `??` — Null Coalescing Operator
```csharp
string input = Console.ReadLine() ?? "0";
// Nếu ReadLine() trả null → dùng "0" thay thế
// Đọc thành câu: "input = ReadLine(), nếu null thì dùng 0"
```

---

### String Interpolation `$""`
```csharp
string name = "Nam";
double gpa = 3.7;

Console.WriteLine($"Tên: {name}, GPA: {gpa:F2}");
// :F2 → format 2 chữ số thập phân (3.7 → 3.70)
// :F0 → làm tròn số nguyên
// :N0 → có dấu phân cách hàng nghìn (1000 → 1,000)
```

---

### `while (true)` + `break`
```csharp
while (true)          // chạy mãi
{
    string input = Console.ReadLine();
    if (input == "exit") break;  // thoát khi cần
}
// Dùng khi không biết trước bao nhiêu lần lặp
```

---

### Xếp loại — Lưu ý thứ tự if/else
```csharp
// ✅ ĐÚNG — kiểm tra từ CAO xuống THẤP
if (diemTB >= 8.5) return "Giỏi";
else if (diemTB >= 7.0) return "Khá";
else if (diemTB >= 5.0) return "Trung bình";
else return "Yếu";

// ❌ SAI — nếu đảo ngược, điểm 9.0 vào "Trung bình" luôn
if (diemTB >= 5.0) return "Trung bình"; // 9.0 >= 5.0 → dừng ở đây!
```

---

### LINQ cơ bản (dùng Day 1)
```csharp
// Sort danh sách theo điểm TB giảm dần
var sorted = danhSach.OrderByDescending(sv => sv.DiemTB).ToList();

// sv => sv.DiemTB là Lambda Expression
// Đọc: "với mỗi sv, lấy sv.DiemTB để so sánh"
```

---


