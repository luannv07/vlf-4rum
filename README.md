# VLF-4RUM

VLF-4RUM là một hệ thống **diễn đàn thảo luận trực tuyến (Web Forum)** cho phép người dùng đăng bài, trao đổi kiến thức và tương tác với nhau thông qua các chủ đề thảo luận.

Dự án được xây dựng bằng **ASP.NET MVC**, sử dụng **SQL Server** để lưu trữ dữ liệu và **Docker** để triển khai môi trường chạy một cách nhất quán.

---

## 🚀 Tính năng chính

- Đăng ký / Đăng nhập tài khoản
- Tạo bài viết (Post)
- Bình luận bài viết
- Quản lý người dùng
- Quản lý bài viết
- Phân loại bài viết theo chủ đề
- Giao diện web đơn giản, dễ sử dụng

---

## 🏗️ Công nghệ sử dụng

### Backend

- ASP.NET Core MVC
- Entity Framework Core

### Database

- Microsoft SQL Server

### DevOps / Environment

- Docker
- Docker Compose

### Công cụ phát triển

- Visual Studio / VS Code
- Git & GitHub
- DBeaver (quản lý database)

---

## 📂 Cấu trúc thư mục (dự kiến)

```
vlf-4rum/
│
├── Controllers/      # Xử lý request từ client
├── Models/           # Entity / Model dữ liệu
├── Views/            # Giao diện MVC (Razor)
├── Data/             # DbContext và cấu hình database
├── wwwroot/          # Static files (css, js, images)
├── appsettings.json  # Cấu hình ứng dụng
├── Program.cs        # Entry point
└── docker-compose.yml
```

---

## ⚙️ Cài đặt và chạy dự án

### 1. Clone project

```bash
git clone https://github.com/<your-username>/vlf-4rum.git
cd vlf-4rum
```

### 2. Chạy database bằng Docker

```bash
docker compose up -d
```

Docker sẽ tự động tạo container **SQL Server**.

---

### 3. Restore package

```bash
dotnet restore
```

---

### 4. Chạy ứng dụng

```bash
dotnet run
```

Sau đó truy cập:

```
http://localhost:5000
```

---

## 🔧 Cấu hình Database

Trong file `appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=forumdb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
}
```

---

## 👥 Thành viên

- Developer: Nguyễn Văn Luận
- Developer: Nguyễn Ngọc Thưởng

---

## 📌 Mục tiêu dự án

- Xây dựng một hệ thống forum cơ bản
- Thực hành ASP.NET MVC
- Làm quen với Docker và SQL Server
- Áp dụng quy trình phát triển phần mềm với Git

---

## 📄 License

Dự án phục vụ mục đích **học tập và nghiên cứu**.
