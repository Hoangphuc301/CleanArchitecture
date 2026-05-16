[11/05/2026]
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

[14/05/2026]
# Clean Architecture + CQRS + FluentValidation + RabbitMQ + MongoDB

> Tổng hợp kiến thức về FluentValidation, MediatR Pipeline, CQRS, RabbitMQ, Eventual Consistency và MongoDB trong ASP.NET Core.

---

# 1. Tại sao FluentValidation phù hợp với Clean Architecture hơn Data Annotations?

## So sánh tổng quan

| Tiêu chí                          | Data Annotations                  | FluentValidation                     |
| --------------------------------- | --------------------------------- | ------------------------------------ |
| Vị trí validation                 | Gắn trực tiếp trên Entity/DTO     | Tách riêng thành Validator class     |
| Separation of Concerns            | Kém, dễ “làm bẩn” model           | Tốt, model sạch sẽ                   |
| Phụ thuộc Framework               | Phụ thuộc ASP.NET/DataAnnotations | Ít phụ thuộc framework               |
| Độ phù hợp với Clean Architecture | Không tối ưu                      | Rất phù hợp                          |
| Validation logic phức tạp         | Khó xử lý                         | Dễ xử lý với `.When()`, `.Must()`    |
| Validation theo điều kiện         | Hạn chế                           | Hỗ trợ mạnh                          |
| Async validation                  | Hầu như không hỗ trợ tốt          | Hỗ trợ `ValidateAsync()`             |
| Inject Service/Repository         | Khó                               | Dễ dàng qua DI                       |
| Tái sử dụng validator             | Thấp                              | Cao                                  |
| Validation theo nhiều ngữ cảnh    | Khó thực hiện                     | Có thể tạo nhiều Validator khác nhau |
| Tích hợp MediatR Pipeline         | Không tối ưu                      | Tích hợp hoàn hảo                    |
| Giữ Handler sạch                  | Handler thường phải check thêm    | Validation tự động trước Handler     |
| Unit Testing                      | Khó test riêng                    | Dễ test độc lập                      |
| Khả năng mở rộng                  | Kém hơn trong project lớn         | Tốt cho enterprise/microservices     |
| Khả năng đọc code                 | Rule bị rải trên model            | Rule tập trung, dễ đọc               |
| Bảo trì lâu dài                   | Khó hơn                           | Dễ maintain                          |
| Hiệu quả trong CQRS               | Không phù hợp lắm                 | Rất phù hợp                          |
| Mức độ linh hoạt                  | Thấp                              | Cao                                  |
| Phù hợp project nhỏ               | Rất phù hợp                       | Có thể hơi dư thừa                   |
| Phù hợp project lớn               | Dễ rối                            | Rất phù hợp                          |

---

# 2. Tích hợp FluentValidation vào MediatR Pipeline

## Kiến trúc hoạt động

```text
API Request
    ↓
MediatR
    ↓
ValidationBehavior
    ↓ valid?
 ┌───────────────┐
 │ YES           │
 │    ↓          │
 │ Handler       │
 └───────────────┘

 ┌───────────────┐
 │ NO            │
 │    ↓          │
 │ Throw Exception
 └───────────────┘
```

---

# 3. Cài đặt Packages

## Application Layer

```bash
dotnet add package FluentValidation.AspNetCore
dotnet add package FluentValidation.DependencyInjectionExtensions
dotnet add package MediatR
```

---

# 4. Tạo Command

```csharp
public record CreateMenuCommand
(
    string MenuName,
    string? Slug,
    int? DisplayOrder
) : IRequest<MenuDto>;
```

---

# 5. Tạo Validator

```csharp
public class CreateMenuValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuValidator()
    {
        RuleFor(x => x.MenuName)
            .NotEmpty()
            .WithMessage("Tên menu là bắt buộc")
            .MaximumLength(100)
            .WithMessage("Tên menu không được vượt quá 100 ký tự");

        RuleFor(x => x.Slug)
            .MaximumLength(500)
            .WithMessage("Đường dẫn không được vượt quá 500 ký tự");

        RuleFor(x => x.DisplayOrder)
            .LessThanOrEqualTo(500)
            .WithMessage("DisplayOrder không được vượt quá 500");
    }
}
```

---

# 6. Tạo ValidationBehavior

```csharp
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
```

---

# 7. Luồng hoạt động

## Khi API gửi request

MediatR sẽ:

1. Chạy `ValidationBehavior`
2. Tìm Validator tương ứng
3. Validate Request
4. Nếu lỗi → throw exception
5. Nếu hợp lệ → gọi Handler

---

# 8. Đăng ký Dependency Injection

## Application Layer

```csharp
services.AddValidatorsFromAssembly(
    Assembly.GetExecutingAssembly());

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        Assembly.GetExecutingAssembly());
});
```

---

## Infrastructure Layer

```csharp
services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));
```

---

## Program.cs

```csharp
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

app.UseMiddleware<ExceptionMiddleware>();
```

---

# 9. Tạo Handler

```csharp
public class CreateMenuHandler
    : IRequestHandler<CreateMenuCommand, MenuDto>
{
    private readonly IMenuRepository _menuRepository;
    private readonly IMapper _mapper;

    public CreateMenuHandler(
        IMenuRepository menuRepository,
        IMapper mapper)
    {
        _menuRepository = menuRepository;
        _mapper = mapper;
    }

    public async Task<MenuDto> Handle(
        CreateMenuCommand request,
        CancellationToken cancellationToken)
    {
        var menuEntity = _mapper.Map<Menus>(request);

        var result = await _menuRepository
            .CreateAsync(menuEntity);

        return _mapper.Map<MenuDto>(result);
    }
}
```

---

# 10. RabbitMQ giải quyết vấn đề gì trong CQRS?

RabbitMQ thường được dùng để:

* Giao tiếp bất đồng bộ giữa các service
* Đồng bộ dữ liệu giữa Write DB và Read DB
* Giảm coupling
* Hỗ trợ Event-Driven Architecture
* Triển khai Eventual Consistency

---

# 11. Eventual Consistency là gì?

> Eventual Consistency là cơ chế đảm bảo dữ liệu cuối cùng sẽ đồng bộ giữa các service, nhưng không nhất thiết phải đồng bộ ngay lập tức.

---

# Luồng hoạt động

```text
Client
   ↓
Create 
   ↓
Write Database updated
   ↓
Publish Event
   ↓
RabbitMQ
   ↓
Consumers
   ↓
Update Read Database
```

---

# Các giai đoạn của Eventual Consistency

## 1. Write Phase

Dữ liệu được ghi vào Write Database.

---

## 2. Inconsistency Window

Có khoảng thời gian ngắn dữ liệu chưa đồng bộ.

---

## 3. Event Propagation

RabbitMQ phát event tới các service khác.

---

## 4. Replication / Update

Các service nhận event và cập nhật dữ liệu.

---

## 5. Final State

Toàn bộ hệ thống trở nên nhất quán.

---

# Ví dụ thực tế

## Facebook / TikTok Like

Bạn nhấn Like:

* Máy bạn thấy tăng ngay
* Người khác có thể thấy chậm vài giây

→ Đây chính là Eventual Consistency.

---

## Chuyển tiền liên ngân hàng

* Tài khoản gửi bị trừ ngay
* Tài khoản nhận cập nhật sau vài phút

→ Cuối cùng dữ liệu vẫn chính xác.

---

# 12. Tại sao MongoDB thường dùng cho Read Side trong CQRS?

Trong CQRS:

* SQL Server thường dùng cho Write Side
* MongoDB thường dùng cho Read Side

---

# Lý do

## 1. Tối ưu truy vấn đọc

SQL Server:

* Dữ liệu chuẩn hóa
* Cần JOIN nhiều bảng

MongoDB:

* Dữ liệu phi chuẩn hóa
* Một Document chứa đủ dữ liệu

---

## Ví dụ

### SQL Server

```sql
SELECT *
FROM Orders o
JOIN Customers c ON ...
JOIN Products p ON ...
```

---

### MongoDB

```js
db.orders.findOne({ _id: orderId })
```

---

# 2. Tốc độ đọc nhanh hơn

MongoDB:

* Tối ưu đọc dữ liệu
* Hỗ trợ index linh hoạt
* Độ trễ thấp

---

# 3. Schema linh hoạt

MongoDB cho phép:

* thêm field mới dễ dàng
* không cần ALTER TABLE

Rất phù hợp khi UI thay đổi liên tục.

---

# 4. Scale tốt hơn

MongoDB hỗ trợ:

* Horizontal Scaling
* Sharding
* Scale-out

---

# 5. Phù hợp với Eventual Consistency

RabbitMQ đẩy Event:

```text
Write Side
    ↓
RabbitMQ
    ↓
Read Side
    ↓
MongoDB updated
```

MongoDB lưu JSON/document rất phù hợp cho Read Model.

---

# So sánh SQL Server vs MongoDB

| Đặc điểm    | SQL Server (Write Side)               | MongoDB (Read Side)    |
| ----------- | ------------------------------------- | ---------------------- |
| Mục tiêu    | Bảo toàn tính toàn vẹn dữ liệu (ACID) | Tối ưu tốc độ hiển thị |
| Cấu trúc    | Chuẩn hóa dữ liệu                     | Phi chuẩn hóa dữ liệu  |
| Truy vấn    | JOIN phức tạp                         | Query đơn giản         |
| Mở rộng     | Scale-up                              | Scale-out              |
| Transaction | Mạnh                                  | Hạn chế hơn            |
| Phù hợp     | Write Side                            | Read Side              |

---

# Tổng kết kiến trúc CQRS hiện đại

```text
Client
   ↓
API
   ↓
MediatR
   ↓
ValidationBehavior
   ↓
Command Handler
   ↓
SQL Server (Write DB)
   ↓
Publish Event
   ↓
RabbitMQ
   ↓
Consumers
   ↓
MongoDB (Read DB)
```

---

# Công nghệ thường dùng

| Công nghệ             | Vai trò         |
| --------------------- | --------------- |
| ASP.NET Core          | Web API         |
| MediatR               | CQRS + Pipeline |
| FluentValidation      | Validation      |
| RabbitMQ              | Message Broker  |
| MongoDB               | Read Database   |
| SQL Server            | Write Database  |
| AutoMapper            | Mapping         |
| Entity Framework Core | ORM             |
```

CleanArchitecture
├── DemoCleanArchitecture.Application
│   ├── Common
│   │   └── Mappings              # Cấu hình AutoMapper (Entity ↔ DTO)
│   │   |    └── ApplicationMappingProfile.cs
│   │   └── Behaviors              # Cấu hình Validation
│   │        └── ValidationBehavior.cs
│   ├── Features                  # Xử lý nghiệp vụ theo từng module (CQRS)
│   │   ├── Menu
│   │   │   ├── Commands          # Logic thay đổi dữ liệu (CUD)
│   │   │   │   └── CreateNew
│   │   │   │       ├── CreateNewCommand.cs
│   │   │   │       └── CreateNewHandler.cs
|   |   |   |       └── CreateMenuValidator.cs
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
    |── Program.cs                # Cấu hình khởi tạo Application
    └── Middleware                #

```
# [16/05/26]
# RabbitMQ + MongoDB Architecture (CQRS Pattern)

## Tổng Quan

Hệ thống sử dụng mô hình:

- Clean Architecture
- CQRS + MediatR
- RabbitMQ
- MongoDB
- SQL Server

Trong đó:

| Thành phần | Vai trò |
| SQL Server | Database chính cho thao tác WRITE |
| MongoDB    | Database phục vụ READ |
| RabbitMQ   | Message Broker truyền Event |

---

#  Kiến Trúc Hệ Thống

```text
[ API (Presentation) ]
            │
            ▼
[ Application Layer ]
            │
            ▼
[ Infrastructure Layer ]
            │
     ┌──────┴──────┐
     ▼             ▼
SQL Server     RabbitMQ
(Write DB)      (Event Bus)
                     │
                     ▼
                Consumer
                     │
                     ▼
                 MongoDB
                 (Read DB)

                [ Domain ]
```

---

#  Luồng Hoạt Động Dữ Liệu

---

#  Luồng Ghi

##  Mục tiêu

Thêm/Sửa/Xóa dữ liệu vào SQL Server và đồng bộ sang MongoDB.

Ví dụ:

```http
POST /api/menu
```

---

##  Flow

```text
Client
   ↓
MenuController
   ↓
MediatR Command
   ↓
ValidationBehavior
   ↓
CreateMenuHandler
   ↓
SQL Server
   ↓
RabbitMQ Publisher
   ↓
RabbitMQ Exchange
   ↓
Queue
   ↓
Consumer
   ↓
MongoDB
```

---

## Các bước xử lý

### 1. API Layer

`MenuController.cs`

- Nhận HTTP Request từ Client
- Tạo `CreateMenuCommand`
- Gọi:

```csharp
await _mediator.Send(command);
```

---

### 2. Validation Pipeline

`ValidationBehavior.cs`

Tự động chạy:

```text
CreateMenuValidator.cs
```

Kiểm tra:

- Required fields
- Format dữ liệu
- Business Rules

| Trạng thái    | Hành động       |
|  Hợp lệ       | Tiếp tục xử lý  |
|  Không hợp lệ | Throw Exception |

---

### 3. Command Handler

`CreateMenuHandler.cs`

Handler thực hiện:

- Tạo Entity
- Gọi Repository
- Lưu vào SQL Server

```text
SQL Server = Source Of Truth
```

---

### 4. Publish Event → RabbitMQ

Sau khi lưu thành công:

```text
MenuCreatedEvent
```

được publish bằng:

```text
RabbitMqPublisher.cs
```

Routing Keys:

```text
menu.created
menu.updated
menu.deleted
```

---

### 5. API Response

Controller trả về:

```http
HTTP 200 / 201
```

---

#  Luồng Đồng Bộ Ngầm

## Mục tiêu

Đồng bộ dữ liệu:

```text
SQL Server → MongoDB
```

theo cơ chế bất đồng bộ.

---

## Flow

```text
RabbitMQ Queue
      ↓
MenuEventConsumer
      ↓
Deserialize Event
      ↓
MongoDB Repository
      ↓
MongoDB Updated
```

---

## Các bước xử lý

### 1. Consumer Lắng Nghe Queue

`MenuEventConsumer.cs`

Lắng nghe queue:

```text
menu-events-queue
```

---

### 2. Nhận Event

Ví dụ message:

```json
{
  "id": 1,
  "name": "Thai Tea",
  "price": 45000
}
```

---

### 3. Parse Message

Consumer sẽ:

- Deserialize JSON
- Xác định loại Event:
  - Created
  - Updated
  - Deleted

---

### 4. Cập Nhật MongoDB

Thông qua:

```text
IMenuReadRepository.cs
```

MongoDB được cập nhật để phục vụ đọc dữ liệu.

---

# Luồng Đọc (Read Flow)

## Mục tiêu

Lấy dữ liệu trực tiếp từ MongoDB.

Ví dụ:

```http
GET /api/menu
```

---

## Flow

```text
Client
   ↓
MenuController
   ↓
GetAllMenuQuery
   ↓
Query Handler
   ↓
MongoDB Repository
   ↓
MongoDB
   ↓
MenuDto
   ↓
Client
```

---

## Các bước xử lý

### 1. API Layer

Controller tạo Query:

```text
GetAllMenuQuery
GetMenuByIdQuery
```

---

### 2. Query Handler

Ví dụ:

```text
GetAllMenuHandler.cs
```

---

### 3. Read Repository

Handler gọi:

```text
IMenuReadRepository.cs
```

---

### 4. MongoDB Query

Infrastructure truy vấn trực tiếp MongoDB.

---

### 5. Mapping DTO

Dữ liệu được map qua:

```text
MenuDto.cs
```

Mục đích:

- Ẩn field không cần thiết
- Tối ưu response
- Trả dữ liệu gọn nhẹ

---

# RabbitMQ Là Gì?

RabbitMQ là:

```text
Message Broker
```

Giúp:

- Gửi message giữa các service
- Tách rời hệ thống
- Xử lý bất đồng bộ
- Queue công việc
- Retry khi lỗi
- Scale hệ thống dễ dàng

---

# MongoDB Là Gì?

MongoDB là:

```text
NoSQL Document Database
```

---

# Tại Sao RabbitMQ Đi Với MongoDB?

Vì chúng giải quyết 2 vấn đề khác nhau.

| Công nghệ | Vai trò |
| RabbitMQ | Truyền dữ liệu/Event |
| MongoDB | Lưu dữ liệu đọc |
| SQL Server | Database giao dịch chính |

---

# RabbitMQ Hoạt Động Như Thế Nào?

RabbitMQ gồm 4 thành phần chính:

| Thành phần | Vai trò            |
| Producer   | Gửi Message        |
| Exchange   | Điều hướng Message |
| Queue      | Hàng đợi Message   | 
| Consumer   | Nhận Message       |

---

# Mapping Với Source Code

| RabbitMQ Component | Source Code  |
| Producer | `RabbitMqPublisher.cs` |
| Exchange | `menu-exchange`        |
| Queue    | `menu-events-queue`    |
| Consumer | `MenuEventConsumer.cs` |

---

# RabbitMQ Event Flow

```text
CreateMenuHandler
        ↓
RabbitMqPublisher
        ↓
Exchange: menu-exchange
        ↓
RoutingKey: menu.created
        ↓
QueueBind
        ↓
Queue: menu-events-queue
        ↓
MenuEventConsumer
        ↓
MongoDB
```

---

# MongoDB Trong CQRS

MongoDB đóng vai trò:

```text
Read Model Database
```

CQRS tách biệt:

| Chức năng | Database |
| WRITE | SQL Server |
| READ | MongoDB |

---

# Lợi Ích Của Kiến Trúc Này

## Performance

- Đọc dữ liệu nhanh hơn
- MongoDB tối ưu query read

---

## Scalability

- Scale Read DB độc lập
- Scale Consumer độc lập

---

## Loose Coupling

Các service không phụ thuộc trực tiếp nhau.

---

## Async Processing

Không block request Client.

---

## Reliability

RabbitMQ giữ message nếu Consumer bị down.

---

# Tổng Kết

Hệ thống sử dụng:

- Clean Architecture
- CQRS + MediatR
- RabbitMQ
- MongoDB
- SQL Server

để xây dựng kiến trúc:

```text
Write Optimized + Read Optimized
```

Giúp hệ thống:

- Dễ mở rộng
- Dễ maintain
- Hiệu năng cao
- Xử lý bất đồng bộ hiệu quả
- Tối ưu truy vấn đọc dữ liệu lớn
```