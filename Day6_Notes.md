# 📚 DAY 6 – SQL Server Cơ Bản & Lệnh CRUD

---

### 1. Tạo Database và Table

**Tạo Database:**
```sql
CREATE DATABASE ShopDB;
GO
USE ShopDB; -- Chuyển sang làm việc trên database vừa tạo
GO
```

**Tạo Bảng (Table):**
```sql
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Categories(Id)
);
GO
```

**Các khái niệm khi tạo bảng:**
- `INT`, `NVARCHAR`, `DECIMAL`: Các kiểu dữ liệu (Số nguyên, Chuỗi có dấu, Số thập phân).
- `IDENTITY(1,1)`: Cột tự động tăng dần, bắt đầu từ 1, bước nhảy là 1. Không cần tự nhập giá trị cho cột này khi thêm dữ liệu.
- `PRIMARY KEY` (Khóa chính): Đánh dấu cột dùng để định danh duy nhất mỗi hàng.
- `FOREIGN KEY REFERENCES` (Khóa ngoại): Tạo mối quan hệ trỏ sang bảng khác. (Ví dụ: `CategoryId` trỏ sang `Id` của bảng `Categories`).
- `NOT NULL`: Cột này bắt buộc phải có dữ liệu.

---

### 2. Các câu lệnh CRUD (Create - Read - Update - Delete)

#### 🔸 INSERT (Thêm dữ liệu)
```sql
-- Thêm vào bảng không có Khóa ngoại
INSERT INTO Categories (Name) 
VALUES 
    (N'Điện tử'), 
    (N'Thời trang');

-- Thêm vào bảng có Khóa ngoại (Giá trị Khóa ngoại PHẢI tồn tại ở bảng cha)
INSERT INTO Products (Name, Price, CategoryId) 
VALUES 
    (N'Laptop', 25000000, 1),
    (N'Áo thun', 150000, 2);
```
*Lưu ý: Chữ `N` trước chuỗi (`N'Laptop'`) dùng để báo cho SQL biết đây là chuỗi Unicode (hỗ trợ tiếng Việt).*

#### 🔸 SELECT (Đọc/Lấy dữ liệu)
```sql
-- Lấy tất cả các cột
SELECT * FROM Products;

-- Lấy một số cột chỉ định
SELECT Id, Name FROM Products;

-- Lọc dữ liệu bằng mệnh đề WHERE
SELECT * FROM Products
WHERE Price > 100000 AND CategoryId = 1;
```

#### 🔸 UPDATE (Sửa dữ liệu)
```sql
-- Tăng giá sản phẩm có Id = 1 lên thành 30000000
UPDATE Products 
SET Price = 30000000 
WHERE Id = 1; 

-- CỰC KỲ QUAN TRỌNG: LUÔN CẦN mệnh đề WHERE trong lệnh UPDATE.
-- Nếu không có WHERE, TẤT CẢ sản phẩm trong bảng sẽ bị sửa!
```

#### 🔸 DELETE (Xóa dữ liệu)
```sql
-- Xóa sản phẩm có Id = 5
DELETE FROM Products 
WHERE Id = 5;

-- CỰC KỲ QUAN TRỌNG: Giống như UPDATE, lệnh DELETE mà không có WHERE sẽ xóa TRẮNG bảng!
```
