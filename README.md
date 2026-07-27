# TaskFlow 🚀

> Full-Stack Task Management App — ASP.NET Core Web API + HTML/CSS/JS

![.NET](https://img.shields.io/badge/.NET-9.0-purple?style=flat-square&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-red?style=flat-square&logo=microsoftsqlserver)
![JWT](https://img.shields.io/badge/Auth-JWT-orange?style=flat-square)
![Font Awesome](https://img.shields.io/badge/Icons-Font_Awesome_6-blue?style=flat-square)

---

## 📌 Giới thiệu

TaskFlow là ứng dụng quản lý công việc cá nhân, được xây dựng với kiến trúc Full-Stack:
- **Backend:** ASP.NET Core Web API (.NET 9) + Entity Framework Core + SQL Server
- **Frontend:** HTML / CSS / JavaScript thuần — kết nối API thật

---

## ✨ Tính năng

| Tính năng | Mô tả |
|-----------|-------|
| 🔐 Đăng ký / Đăng nhập | JWT Authentication, hash password SHA256 |
| 📋 Quản lý task | Tạo, sửa, xóa, đánh dấu hoàn thành |
| 🔒 Phân quyền dữ liệu | Mỗi user chỉ thấy task của mình |
| 📊 Thống kê | Tổng task, đang làm, hoàn thành |
| 🔍 Lọc task | Tất cả / Đang làm / Hoàn thành |
| 🎨 UI hiện đại | Dark theme, Font Awesome, responsive |

---

## 🏗️ Kiến trúc Backend

```
Controller  →  Service  →  Repository  →  Database
   ↑               ↑
  DTO           Business Logic
```

```
TaskApi/
├── Controllers/
│   ├── AuthController.cs     # POST /api/auth/register, /login
│   └── TasksController.cs    # CRUD /api/tasks
├── Services/
│   ├── AuthService.cs        # JWT Token, Hash Password
│   └── TaskService.cs        # Business Logic
├── Models/
│   ├── User.cs
│   └── TaskItem.cs
├── DTOs/
│   ├── AuthDto.cs            # RegisterDto, LoginDto, TokenResponseDto
│   └── TaskDto.cs            # TaskCreateDto, TaskUpdateDto, TaskResponseDto
├── Data/
│   └── AppDbContext.cs
└── Program.cs                # DI, JWT, CORS, Swagger
```

---

## 🖥️ Cấu trúc Frontend

```
TaskFlow-UI/
├── login.html
├── register.html
├── tasks.html
├── css/
│   ├── login.css
│   ├── register.css
│   └── tasks.css
└── js/
    ├── login.js
    ├── register.js
    └── tasks.js
```
---

## 📊 Sơ đồ hệ thống

### 1. Sequence Diagram — Đăng nhập

```mermaid
sequenceDiagram
    actor User
    participant UI as Frontend
    participant API as Task API
    participant Auth as AuthService
    participant DB as Database

    User->>UI: Nhập username/password
    UI->>API: POST /api/auth/login
    API->>Auth: LoginAsync(dto)
    Auth->>DB: Tìm user theo username
    DB-->>Auth: User data
    Auth->>Auth: Verify password hash
    Auth-->>API: Trả về JWT token
    API-->>UI: 200 OK + token
    UI->>User: Lưu token, chuyển sang trang task
```

### 2. Sequence Diagram — Tạo task

```mermaid
sequenceDiagram
    actor User
    participant UI as Frontend
    participant API as Task API
    participant TaskS as TaskService
    participant DB as Database

    User->>UI: Nhấn Thêm task
    UI->>API: POST /api/tasks + Bearer Token
    API->>API: Kiểm tra JWT token → lấy userId
    API->>TaskS: CreateAsync(task)
    TaskS->>DB: INSERT TaskItem
    DB-->>TaskS: Task được lưu
    TaskS-->>API: Trả task mới
    API-->>UI: 201 Created
    UI->>User: Hiển thị task mới
```

### 3. Activity Diagram — Luồng tạo task

```mermaid
flowchart TD
    A([User mở trang task]) --> B[Nhấn nút Thêm task]
    B --> C[Frontend mở modal nhập thông tin]
    C --> D[User nhập tiêu đề và mô tả]
    D --> E[Frontend gửi POST /api/tasks với JWT]
    E --> F{Dữ liệu hợp lệ?}
    F -- Không --> G[API trả lỗi 400]
    G --> H[Hiển thị thông báo lỗi]
    H --> D
    F -- Có --> I[API xác thực JWT và lấy userId]
    I --> J[TaskService tạo task mới]
    J --> K[(Database lưu task)]
    K --> L[API trả 201 + task vừa tạo]
    L --> M[Frontend cập nhật danh sách]
    M --> N([Hiển thị task mới trên màn hình])
```

### 4. ERD — Quan hệ Database

```mermaid
erDiagram
    USER ||--o{ TASK : owns
    USER {
        int Id PK
        string Username
        string PasswordHash
        string Role
    }
    TASK {
        int Id PK
        string Title
        string Description
        bool IsCompleted
        datetime CreatedAt
        int UserId FK
    }
```

### 5. Component Diagram — Kiến trúc tổng thể

```mermaid
flowchart LR
    subgraph Frontend ["🖥️ TaskFlow-UI (Browser)"]
        HTML["HTML / CSS / JS"]
        LS["LocalStorage\n(JWT Token)"]
    end

    subgraph Backend ["⚙️ Task API (ASP.NET Core)"]
        Auth["AuthService\n(Register/Login/Token)"]
        TaskS["TaskService\n(CRUD Tasks)"]
        EF["EF Core"]
    end

    DB[("🗄️ SQL Server\nTaskApiDB")]

    HTML -->|"HTTPS /api/auth"| Auth
    HTML -->|"HTTPS /api/tasks\n+ Bearer Token"| TaskS
    HTML <-->|"read/write token"| LS
    Auth --> EF
    TaskS --> EF
    EF --> DB
```

---

## 🚀 Cài đặt và chạy

### Yêu cầu
- .NET 9 SDK
- SQL Server 2022
- Visual Studio 2022

### Backend
```bash
# 1. Clone repo
git clone https://github.com/YOUR_USERNAME/taskflow.git

# 2. Mở TaskApi trong Visual Studio

# 3. Sửa connection string trong Program.cs
Server=.\TEN_SERVER; Database=TaskApiDB; Trusted_Connection=True; TrustServerCertificate=True;

# 4. Chạy Migration
Add-Migration InitTaskDb
Update-Database

# 5. Chạy API (F5) → mở Swagger tại https://localhost:{port}/swagger
```

### Frontend
```bash
# Sửa port trong 3 file js/
const API_URL = 'https://localhost:{port}/api';

# Mở login.html trong trình duyệt
```

---

## 📡 API Endpoints

| Method | URL | Mô tả | Auth |
|--------|-----|-------|------|
| POST | `/api/auth/register` | Đăng ký | ❌ |
| POST | `/api/auth/login` | Đăng nhập → JWT Token | ❌ |
| GET | `/api/tasks` | Lấy danh sách task của mình | ✅ |
| GET | `/api/tasks/{id}` | Lấy 1 task | ✅ |
| POST | `/api/tasks` | Tạo task mới | ✅ |
| PUT | `/api/tasks/{id}` | Cập nhật task | ✅ |
| DELETE | `/api/tasks/{id}` | Xóa task | ✅ |

---

## 🛠️ Tech Stack

| Layer | Công nghệ |
|-------|-----------|
| Backend | ASP.NET Core 9, C# |
| ORM | Entity Framework Core 9 |
| Database | SQL Server 2022 |
| Auth | JWT Bearer Token |
| API Docs | Swagger / OpenAPI |
| Frontend | HTML5, CSS3, JavaScript ES6 |
| Icons | Font Awesome 6 |
| Fonts | Google Fonts (Inter) |

---

## 👨‍💻 Tác giả

Được xây dựng trong lộ trình học **Backend .NET 14 ngày**.
