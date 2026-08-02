# 🌌 .NET Clean API
### Powered by **Singleteam (Sglt)** & **Peak CLI**

<div align="left">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET" />
  <img src="https://img.shields.io/badge/Architecture-Clean-blue?style=for-the-badge" alt="Clean Architecture" />
  <img src="https://img.shields.io/badge/Status-Scaffolded-success?style=for-the-badge" alt="Status" />
</div>

---

## 📐 Architecture Blueprint

This solution implements a strict **Onion Architecture** pattern, ensuring your business logic remains independent, testable, and scalable.

* **`Shared`** — Enterprise logic, Entities, and Value Objects.
* **`Usecase`** — Use Cases, DTOs, and MediatR Commands/Queries.
* **`Data`** — Persistence layer, EF Core configurations, and Migrations.
* **`Api`** — Controllers, Middleware, and Dependency Injection.

---

## ⚡ Quick Start

Get your environment up and running in seconds:

```bash
# 1. Create a new database and update appsettings.json
# 2. Update appsettings.json and appsettings.Development.json with secret keys
# 3. Execute the following command
dotnet ef database update --project Data --startup-project Api

# 4 Run the application
```