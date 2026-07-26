# 📚 DAY 7 – SQL JOIN & Thống Kê (GROUP BY, HAVING)

---

### 1. KẾT NỐI BẢNG (JOIN)

`JOIN` dùng để gộp dữ liệu từ 2 hay nhiều bảng lại với nhau dựa trên Khóa ngoại (Foreign Key) và Khóa chính (Primary Key).

#### 🔸 INNER JOIN
Chỉ lấy những dòng có dữ liệu khớp nhau ở cả 2 bảng.
```sql
SELECT 
    p.Name AS ProductName, 
    p.Price, 
    c.Name AS CategoryName
FROM Products p
INNER JOIN Categories c 
    ON p.CategoryId = c.Id;
```
*Giải thích:*
- `p` và `c` là bí danh (alias) giúp code ngắn gọn hơn.
- `ON p.CategoryId = c.Id` là điều kiện khớp nối: Mã danh mục bên Sản phẩm phải bằng Mã danh mục bên bảng Danh mục.

#### 🔸 LEFT JOIN
Lấy TẤT CẢ các dòng ở bảng bên trái (bảng ghi sau chữ `FROM`), nếu bảng bên phải không có dữ liệu khớp thì cột bên phải sẽ hiển thị `NULL`.
```sql
SELECT c.Name AS CategoryName, p.Name AS ProductName
FROM Categories c
LEFT JOIN Products p
    ON c.Id = p.CategoryId;
```

---

### 2. THỐNG KÊ & GOM NHÓM (GROUP BY)

#### 🔸 Các hàm Thống kê (Aggregate Functions)
Thường dùng để tính toán trên nhiều dòng:
- `COUNT(Id)`: Đếm số lượng
- `SUM(Price)`: Tính tổng
- `AVG(Price)`: Tính trung bình
- `MAX(Price)` / `MIN(Price)`: Lớn nhất / Nhỏ nhất

#### 🔸 Mệnh đề GROUP BY
Dùng để gom các dòng có chung một giá trị nào đó thành từng nhóm, sau đó dùng hàm thống kê tính toán cho nhóm đó.
*(Lưu ý: Bất kỳ cột nào nằm ở `SELECT` mà không phải là hàm thống kê thì BẮT BUỘC phải nằm trong mệnh đề `GROUP BY`)*

```sql
-- Đếm số sản phẩm trong mỗi Danh mục
SELECT 
    CategoryId, 
    COUNT(Id) AS TotalProducts
FROM Products
GROUP BY CategoryId;
```

#### 🔸 Mệnh đề HAVING
Giống như `WHERE`, nhưng `HAVING` dùng để lọc kết quả **SAU KHI đã gom nhóm** (GROUP BY).
```sql
-- Đếm số sản phẩm trong mỗi Danh mục, NHƯNG CHỈ hiển thị danh mục có 2 sản phẩm trở lên
SELECT 
    CategoryId, 
    COUNT(Id) AS TotalProducts
FROM Products
GROUP BY CategoryId
HAVING COUNT(Id) >= 2;
```

---

### 3. TỔNG KẾT THỨ TỰ THỰC THI (QUAN TRỌNG)
Khi bạn gõ một câu SQL dài, CSDL sẽ đọc và thực thi theo thứ tự sau chứ không phải từ trên xuống dưới:
1. `FROM` / `JOIN` (Xác định lấy từ bảng nào)
2. `WHERE` (Lọc từng dòng)
3. `GROUP BY` (Gom nhóm)
4. `HAVING` (Lọc từng nhóm)
5. `SELECT` (Chọn cột hiển thị ra màn hình)
6. `ORDER BY` (Sắp xếp kết quả cuối cùng)
