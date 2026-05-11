# Learning Clean Architecture + CQRS + MediatR

## Progress Table 

| STT | Công việc                                                                       |   Trạng thái    |
| --- | ------------------------------------------------------------------------------- |   ----------    |
| 1   | Nghiên cứu kiến trúc Clean Architecture và lý do Tight Coupling được giải quyết |    Đã xong      |
| 2   | Phân tích nguyên lý SOLID trong Clean Architecture                              |    Đã xong      |
| 3   | Tìm hiểu Mediator Pattern với MediatR                                           |    Đã xong      |
| 4   | Nghiên cứu sự kết hợp giữa CQRS và Mediator Pattern                             |    Đã xong      |
| 5   | SQL Server + EF Core (DB First) + LINQ                                          |    Đã xong      |
| 6   | Xây dựng project theo kiến trúc Clean Architecture + CQRS + Mediator            | Đang hoàn thiện |

---
####  Thách thức & Khó khăn (Issues)
* **Đồng bộ DB First:** Tool scaffold thường sinh ra các class Models trực tiếp. Nếu để chúng ở tầng Infrastructure thì vi phạm Clean Architecture (vì Domain mới là nơi chứa Entity)
* **Quản lý Boilerplate:** Số lượng file tăng nhanh do cấu trúc tách biệt của CQRS và Mediator

# 1. Clean Architecture

## Clean Architecture là gì?

Clean Architecture là một kiến trúc phần mềm tập trung vào việc tách biệt các mối quan tâm (Separation of Concerns), giúp hệ thống:

- Dễ bảo trì
- Dễ mở rộng
- Dễ test
- Dễ tái sử dụng

Nguyên tắc cốt lõi là:

> Dependency Rule – sự phụ thuộc chỉ hướng vào bên trong.

Các tầng bên trong không phụ thuộc vào các tầng bên ngoài.

Application và Domain không biết:

- Database
- Framework
- UI

Chúng chỉ làm việc thông qua Interface.

---

## Các tầng trong Clean Architecture

### Domain

- Không phụ thuộc bất kỳ tầng nào
- Là tầng cốt lõi của hệ thống
- Chứa:
  - Entity
  - Value Object
  - Business Rules
  - Interface Abstraction

---

### Application

- Phụ thuộc vào Domain
- Chứa các Use Case
- Điều phối dữ liệu giữa các tầng

Bao gồm:

- DTOs:
  dùng để trao đổi dữ liệu giữa các tầng
- Validators:
  kiểm tra dữ liệu đầu vào
- Handlers:
  xử lý nghiệp vụ thông qua Interface mà không phụ thuộc implementation cụ thể

---

### Presentation

- Là tầng giao diện:
  - Web API
  - WPF
  - MVC

- Nhận request từ người dùng
- Gửi Command/Query đến Application
- Chủ yếu phụ thuộc vào Application

---

### Infrastructure

- Implement các Interface
- Chứa:
  - Repository
  - EF Core
  - Database
  - Email Service
  - File System
  - Identity

---

# 2. Tại sao Tight Coupling được giải quyết bằng Clean Architecture?

## Tight Coupling là gì?

Là khi các thành phần trong hệ thống phụ thuộc trực tiếp vào nhau.

Ví dụ:

```csharp
public class UserService
{
    private SqlUserRepository repo = new SqlUserRepository();
}
```

`UserService` phụ thuộc trực tiếp vào `SqlUserRepository`.

Nếu đổi Database:

- SQL Server
- MongoDB

=> phải sửa business logic.

Điều này khiến hệ thống:

- khó mở rộng
- khó bảo trì
- khó test

---

## Clean Architecture giải quyết như thế nào?

### 1. Tách hệ thống thành các Layer

- Domain
- Application
- Presentation
- Infrastructure

Mỗi layer có trách nhiệm riêng.

---

### 2. Áp dụng Dependency Rule

Các tầng bên trong:

- không biết UI
- không biết Database
- không biết Framework

---

### 3. Sử dụng Interface thay vì Implementation

Thay vì:

```csharp
SqlUserRepository repo = new SqlUserRepository();
```

Sử dụng:

```csharp
IUserRepository repo;
```

Application chỉ làm việc với Interface.

---

### 4. Infrastructure Implement Interface

```csharp
public interface IUserRepository
{
    void Save(User user);
}
```

```csharp
public class SqlUserRepository : IUserRepository
{
    public void Save(User user)
    {
        
    }
}
```

Điều này giúp:

- Business Logic không phụ thuộc Database
- Có thể thay đổi implementation dễ dàng

---

### 5. Dependency Inversion Principle (DIP)

> High Level Modules should not depend on Low Level Modules

Application
    ↓
Interface
    ↑
Infrastructure

- Application chỉ biết Interface
- Infrastructure implement Interface

=> giúp giảm Tight Coupling.

---

# 3. SOLID trong Clean Architecture

## S — Single Responsibility Principle (SRP)

Mỗi class/file chỉ nên làm một việc.

Ví dụ:

`CreateNewsHandler`
chỉ dùng để tạo tin tức,
không nên:

- gửi mail
- xóa tin

---

## O — Open/Closed Principle (OCP)

Có thể mở rộng mà không sửa code cũ.

Ví dụ:

- đổi SQL Server → MongoDB
- chỉ cần thêm implementation mới

Không sửa business logic.

---

## L — Liskov Substitution Principle (LSP)

Class con phải thay thế được class cha.

Ví dụ:

```csharp
IUserRepository
```

được implement bởi:

- SqlUserRepository
- MongoUserRepository

Application vẫn hoạt động đúng.

---

## I — Interface Segregation Principle (ISP)

Mỗi Interface chỉ nên phục vụ một chức năng.

Không nên:

```csharp
IRepository
- Save()
- SendMail()
```

Nên tách:

```csharp
INewsRepository
IEmailService
```

---

## D — Dependency Inversion Principle (DIP)

Nguyên lý quan trọng nhất trong Clean Architecture.

Application
    ↓
Interface
    ↑
Infrastructure

- Application chỉ biết Interface
- Infrastructure implement Interface

=> giảm Tight Coupling.

---

# 4. Mediator Pattern với MediatR

## Mediator Pattern là gì?

Mediator Pattern giúp giảm sự giao tiếp trực tiếp giữa các object bằng cách thông qua một Mediator.

Thay vì:

```text
Controller → Service → Repository
```

sẽ thành:

```text
Controller → Mediator → Handler
```

Mediator sẽ điều phối request đến đúng Handler.

---

## MediatR là gì?

MediatR là thư viện .NET dùng để triển khai Mediator Pattern.

Bao gồm:

- Request / Command / Query
- Handler
- MediatR

---

## Lợi ích

- Giảm Coupling
- Dễ mở rộng
- Dễ maintain
- Mỗi Handler xử lý đúng một nghiệp vụ

---

# 5. CQRS kết hợp với Mediator Pattern

## CQRS là gì?

CQRS = Command Query Responsibility Segregation (Phân tách trách nhiệm truy vấn lệnh)

Chia hệ thống thành:

### Command

Dùng để:

- Create
- Update
- Delete

---

### Query

Dùng để:

- Get
- Search
- GetById

---

## CQRS + Mediator

Mediator sẽ:

- nhận Command/Query
- chuyển tới Handler tương ứng

Luồng hoạt động:

```text
User Request
    ↓
Controller
    ↓
Mediator.Send()
    ↓
Command / Query
    ↓
Handler
    ↓
Repository Interface
    ↓
Infrastructure
    ↓
Database
```

---

# 6. SQL Server + EF Core + LINQ

## SQL Server

Là hệ quản trị cơ sở dữ liệu dùng để lưu trữ dữ liệu.

---

## EF Core

Là ORM hỗ trợ thao tác Database bằng object C# thay vì viết SQL thủ công.

---

## DB First

Tạo Database trước, sau đó dùng EF Core sinh Entity và DbContext.

---

## LINQ

LINQ (Language Integrated Query) là cách truy vấn dữ liệu bằng cú pháp C#.

---

## Các kiểu LINQ phổ biến

### Method Syntax

Sử dụng:

- `.Where()`
- `.Select()`
- `.OrderBy()`
- `.FirstOrDefault()`

Ví dụ:

```csharp
return await _architectureDbContext.Menus
    .Where(x => x.MenuId == id)
    .FirstOrDefaultAsync();
```

---

### Query Syntax

Cú pháp gần giống SQL.

Ví dụ:

```csharp
var result = from n in _architectureDbContext.News
             where n.IsActive == true
             select n;
```

# Xây dựng Project theo Clean Architecture + CQRS + MediatR

## Cấu trúc tổng quát

```text
CleanArchitecture
├── DemoCleanArchitecture.Application
│   ├── Common
│   │   └── Mappings              # Cấu hình AutoMapper (Entity ↔ DTO)
│   │       └── ApplicationMappingProfile.cs
│   ├── Features                  # Xử lý nghiệp vụ theo từng module (CQRS)
│   │   ├── Menu
│   │   │   ├── Commands          # Logic thay đổi dữ liệu (CUD)
│   │   │   │   └── CreateNew
│   │   │   │       ├── CreateNewCommand.cs
│   │   │   │       └── CreateNewHandler.cs
│   │   │   └── Queries           # Logic truy vấn dữ liệu (R)
│   │   │       ├── GetAllNew
│   │   │       └── GetNewById
│   │   └── News                  # Module xử lý tin tức
│   │       └── DTOs              # Vật chứa dữ liệu trao đổi giữa các tầng
│   │           └── NewDTO.cs
│   └── DependencyInjection.cs    # Đăng ký Service tầng Application
├── DemoCleanArchitecture.Domain
│   ├── Entities                  # Thực thể nghiệp vụ chính
│   │   ├── MenuNews.cs
│   │   ├── Menus.cs
│   │   └── News.cs
│   └── Interfaces                # Định nghĩa các bản thiết kế (Abstraction)
│       ├── IMenuRepository.cs
│       └── INewRepository.cs
├── DemoCleanArchitecture.Infrastructure
│   ├── Data                      # Kết nối Database (EF Core)
│   │   └── ArchitectureDbContext.cs
│   ├── Repositories              # Triển khai chi tiết truy vấn DB
│   │   ├── MenuRepository.cs
│   │   └── NewRepository.cs
│   └── DependencyInjection.cs    # Đăng ký Repository & Infrastructure Service
└── DemoCleanArchitecture.API
    ├── Controllers               # Tiếp nhận Request từ Client
    └── Program.cs                # Cấu hình khởi tạo Application
```
