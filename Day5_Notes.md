## DAY 5 – Async / Await

### Tại sao cần Async?
> Khi ứng dụng gọi Database hoặc API bên ngoài, nó phải chờ đợi.
> Nếu dùng **Synchronous (đồng bộ)**: Thread hiện tại bị "đóng băng" (block) không làm gì được cho đến khi có kết quả.
> Nếu dùng **Asynchronous (bất đồng bộ)**: Thread hiện tại được giải phóng để làm việc khác. Khi nào có kết quả, sẽ có thread khác tiếp tục xử lý.
> 👉 **Tăng cường hiệu năng và khả năng chịu tải (throughput) cho Web Server.**

---

### `async` và `await`
```csharp
// Method Async luôn trả về Task hoặc Task<T>
// Đặt tên method luôn có hậu tố "Async"
public async Task<List<Product>> FetchProductsAsync()
{
    Console.WriteLine("Bắt đầu fetch data...");

    // Task.Delay giả lập quá trình chờ (chờ Database, gọi API...)
    // await: Đợi cho đến khi quá trình này xong, NHƯNG KHÔNG block thread
    await Task.Delay(2000); 

    Console.WriteLine("Đã lấy được data!");

    return new List<Product> 
    { 
        new Product(1, "Laptop", 25000000, "Tech") 
    };
}
```

---

### Cách gọi phương thức Async

```csharp
// Trong method có chữ 'async', bạn được phép dùng chữ 'await'
public static async Task Main()
{
    Console.WriteLine("1. Đang làm việc A");

    // Gọi method async và ĐỢI kết quả (không block)
    List<Product> list = await FetchProductsAsync();

    Console.WriteLine("2. Đang làm việc B");
}
```

### Các quy tắc ngầm định (Best Practices)
1. **Async all the way**: Đã dùng async thì các tầng trên cũng phải async. Đừng gọi hàm async từ hàm sync nếu không bắt buộc (dễ gây Deadlock).
2. **Return `Task` thay vì `void`**: Trong method async, không trả về gì thì dùng `Task`, không dùng `void` (trừ event handlers).
3. **Task.Run()**: Dùng để đẩy 1 công việc nặng (tính toán, xử lý file lớn) sang thread nền (background thread), tránh làm đơ giao diện.

---

### 🚨 Rút kinh nghiệm từ Mini Review

**1. Unreachable Code (Code không bao giờ chạy tới)**
```csharp
public double GetAverageGPA()
{   
    var tb = _students.Average(s => s.GPA);
    return tb; // <--- HÀM KẾT THÚC NGAY TẠI ĐÂY!

    // ĐOẠN CODE BÊN DƯỚI BỊ BỎ QUA HOÀN TOÀN:
    if (tb == 0) throw new Exception();
}
```
👉 **Cách Fix:** Khi gặp lệnh `return` hoặc `throw`, hàm sẽ lập tức thoát ra. Do đó, mọi validation (kiểm tra điều kiện) phải nằm **TRÊN** lệnh `return`. Thêm nữa, LINQ `.Average()` sẽ tự động báo lỗi nếu danh sách rỗng, nên ta phải check `.Count == 0` ngay từ đầu hàm.

**2. Ném và Bắt Exception Cụ Thể**
```csharp
// ❌ Khi ném lỗi, không nên dùng lỗi chung chung của hệ thống nếu đã tự tạo lỗi riêng
if (student == null) throw new NotImplementedException(); 

// ✅ Ném đúng lỗi tự định nghĩa
if (student == null) throw new StudentNotFoundException(id);
```

```csharp
// ❌ Khi bắt lỗi, nếu bắt Exception chung chung sẽ chụp luôn các lỗi hệ thống không mong muốn
catch (Exception ex) { ... }

// ✅ Chỉ bắt đúng cái lỗi mà bạn chủ đích văng ra
catch (StudentNotFoundException ex) { ... }
```
