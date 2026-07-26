## DAY 3 – OOP: Inheritance, Abstract, Interface

### Inheritance (Kế thừa)
```csharp
class Animal              // class cha
{
    public string Name { get; set; }
    public void Eat() { Console.WriteLine($"{Name} đang ăn"); }
}

class Dog : Animal        // Dog KẾ THỪA Animal (dùng dấu :)
{
    public void Bark() { Console.WriteLine("Gâu gâu!"); }
}

Dog d = new Dog { Name = "Milo" };
d.Eat();   // ← lấy từ Animal, không cần viết lại
d.Bark();  // ← của riêng Dog
```

---

### `virtual` + `override` — Ghi đè hành vi
```csharp
class Animal
{
    public virtual void Speak()       // virtual = "con có thể ghi đè"
    {
        Console.WriteLine("...");
    }
}

class Dog : Animal
{
    public override void Speak()      // override = "tao ghi đè"
    {
        Console.WriteLine("Gâu gâu!");
    }
}

// Polymorphism — cùng gọi Speak() nhưng khác kết quả
List<Animal> zoo = new() { new Dog(), new Cat() };
foreach (var a in zoo)
    a.Speak();  // Dog→ Gâu gâu!  Cat→ Meo meo!
```

---

### `abstract` class
```csharp
abstract class Animal                 // không thể new Animal() trực tiếp
{
    public string Name { get; set; }
    public abstract void Speak();     // bắt buộc class con override, không có {}
    public void Eat() { ... }         // method bình thường, class con dùng luôn
}

// Animal a = new Animal(); ← ❌ LỖI
Dog d = new Dog();                    // ✅
```

| | abstract | virtual |
|--|----------|---------|
| Class con bắt buộc override? | ✅ Có | ❌ Không |
| Có thể có code trong method? | ❌ Không | ✅ Có |

---

### Intdaerface
```csharp
interface IPayable                    // I prefix = convention
{
    double CalculateSalary();         // không có thân {}, không có access modifier
    void PrintPayslip();
}

class FullTimeEmployee : IPayable    // implement interface
{
    public string Name { get; set; }
    public double MonthlySalary { get; set; }

    public double CalculateSalary() => MonthlySalary;          // ✅ bắt buộc implement
    public void PrintPayslip() => Console.WriteLine($"{Name}: {CalculateSalary():N0}");
}
```

---

### Abstract Class vs Interface

| | Abstract Class | Interface |
|--|----------------|-----------|
| Có code thật không? | ✅ Có thể có | ❌ Thuần contract |
| Kế thừa bao nhiêu? | Chỉ 1 class | Nhiều interface |
| Dùng khi nào? | Có code chung cần tái sử dụng | Chỉ định "phải làm gì" |
| Ví dụ | `Animal` → `Dog`, `Cat` | `IPayable`, `ILogger`, `IRepository` |

---

### Lỗi hay gặp Day 3
```csharp
// ❌ VS2022 tự tạo class khi Add New Item
internal class IPayable { }

// ✅ Phải tự sửa thành interface
interface IPayable { }

// ❌ Thiếu () khi gọi method trong string interpolation
Console.WriteLine($"Lương: {CalculateSalary}");   // in ra tên method

// ✅ Phải có ()
Console.WriteLine($"Lương: {CalculateSalary()}"); // gọi method, in ra giá trị

// ❌ Code thực thi nằm ngoài class
using MyApp;
Console.WriteLine("hello");   // ← nằm ngoài class → lỗi
public class Program { ... }

// ✅ Phải nằm trong Main
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("hello"); // ✅
    }
}
```

---

### `15_000_000` — Số dễ đọc
```csharp
// C# cho phép dùng _ trong số để dễ đọc, không ảnh hưởng giá trị
double luong = 15_000_000;   // = 15000000
int max = 1_000_000;         // = 1000000
```

---


