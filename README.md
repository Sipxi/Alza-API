# AlzaApi

REST API for product management built with ASP.NET Core. Supports v1 (classic) and v2 (paginated) endpoints. Uses SQL Server for data storage with Entity Framework Core. Includes unit, integration, and end-to-end tests.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Git](https://git-scm.com/)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd alza-api/AlzaApi
```

### 2. Configure the database connection

Update the connection string in `AlzaApi.App/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AlzaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Run the application

```bash
cd AlzaApi.App
dotnet run
```

The application will automatically apply migrations and seed the database on first run.

### 4. Open API documentation

Navigate to [http://localhost:5000/swagger](http://localhost:5000/swagger) in your browser.

Two API versions are available:
- **V1** — Classic endpoints (get all, get by id, update description)
- **V2** — Paginated endpoints

---

## Running with Mock Data (no database required)

Set `UseMockData` to `true` in `appsettings.json`:

```json
{
  "UseMockData": true
}
```

This uses in-memory seed data instead of SQL Server.

---

## Running Tests

### Unit Tests (BL layer)

No database required — uses Moq.

```bash
cd AlzaApi.BL.UnitTests
dotnet test
```

### Integration Tests (DAL layer)

Requires a SQL Server instance. Create `appsettings.Test.json` in `AlzaApi.DAL.IntegrationTests/`:

```json
{
  "ConnectionStrings": {
    "TestConnection": "Server=YOUR_SERVER;Database=AlzaDb_Test;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Then run:

```bash
cd AlzaApi.DAL.IntegrationTests
dotnet test
```

### End-to-End Tests (APP layer)

Requires a SQL Server instance. Create `appsettings.Test.json` in `AlzaApi.App.EndToEndTests/`:

```json
{
  "ConnectionStrings": {
    "TestConnection": "Server=YOUR_SERVER;Database=AlzaDb_Test;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Then run:

```bash
cd AlzaApi.App.EndToEndTests
dotnet test
```

> **Note:** `appsettings.Test.json` files are excluded from version control. Create them locally before running tests.

### Run all tests at once

```bash
dotnet test
```

---

## Project Structure

```
AlzaApi/
├── AlzaApi.App/                  # ASP.NET Core application, controllers, middleware
├── AlzaApi.BL/                   # Business logic layer, services
├── AlzaApi.DAL/                  # Data access layer, repositories, migrations
├── AlzaApi.Common/               # Shared DTOs, models, exceptions
├── AlzaApi.BL.UnitTests/         # Unit tests for BL layer (Moq)
├── AlzaApi.DAL.IntegrationTests/ # Integration tests for DAL layer
└── AlzaApi.App.EndToEndTests/    # End-to-end tests for API endpoints
```

## API Overview

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/products` | Get all products |
| GET | `/api/v1/products/{id}` | Get product by ID |
| PATCH | `/api/v1/products/{id}/description` | Update product description |
| GET | `/api/v2/products` | Get paginated products |