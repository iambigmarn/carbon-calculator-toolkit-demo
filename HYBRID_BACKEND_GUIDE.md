# Carbon Calculator Toolkit - Hybrid Backend Implementation

## Overview

This project demonstrates a **hybrid approach** with both a **Node.js mock API** (for immediate demo) and a **C#/.NET Core Web API** (for production-ready implementation). This showcases both rapid prototyping capabilities and enterprise-grade C#/.NET development skills.

## Architecture

### Current Implementation (Mock Backend)
- **Backend**: Node.js/Express mock API server
- **Database**: In-memory JavaScript arrays
- **Frontend**: Next.js/React with TypeScript
- **Purpose**: Quick demo and prototyping

### Production Implementation (C#/.NET Backend)
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Database**: MS-SQL Server with Entity Framework Core
- **Architecture**: Clean Architecture (Core, Infrastructure, API layers)
- **Frontend**: Next.js/React with TypeScript
- **Purpose**: Production-ready, scalable implementation

## Project Structure

```
carbon-calculator-toolkit-demo/
├── frontend/                          # Next.js React frontend
│   ├── src/
│   │   ├── app/                      # Next.js 13+ app router
│   │   └── components/               # React components
│   ├── backend-config.json           # Backend switching configuration
│   └── next.config.ts               # Next.js configuration
├── backend/                          # C#/.NET Core backend
│   ├── CarbonCalculator.API/         # Web API layer
│   │   ├── Controllers/              # API controllers
│   │   ├── Program.cs               # Application entry point
│   │   └── appsettings.json         # Configuration
│   ├── CarbonCalculator.Core/        # Domain layer
│   │   ├── Entities/                # Domain models
│   │   ├── Interfaces/              # Service interfaces
│   │   └── Services/                # Business logic
│   └── CarbonCalculator.Infrastructure/ # Data layer
│       └── Data/                    # Entity Framework context
├── mock-api-server.js               # Node.js mock API server
├── database/                        # SQL scripts and documentation
└── docs/                           # Project documentation
```

## C#/.NET Backend Features

### 🏗️ **Clean Architecture**
- **Core Layer**: Domain entities, interfaces, and business logic
- **Infrastructure Layer**: Data access with Entity Framework Core
- **API Layer**: Controllers, DTOs, and HTTP endpoints

### 🗄️ **MS-SQL Server Database**
- **Entity Framework Core** for ORM
- **Code-First** approach with migrations
- **Normalized database schema** with proper relationships
- **Indexes** for performance optimization

### 🔧 **Key Technologies**
- **ASP.NET Core 8.0** Web API
- **Entity Framework Core 8.0** with MS-SQL Server
- **Dependency Injection** for service management
- **CORS** configuration for frontend integration
- **Swagger/OpenAPI** for API documentation
- **Structured Logging** with ILogger

### 📊 **Database Schema**
```sql
-- Core Tables
EmissionFactors          -- Carbon emission factors
MitigationStrategies     -- Reduction strategies
Calculations            -- Main calculation records

-- Related Tables
CalculationActivities   -- Activity breakdowns
CalculationHotspots     -- High-emission areas
CalculationMitigationStrategies -- Applied strategies
```

## API Endpoints

### Calculator Controller (`/api/calculator`)
- `POST /calculate` - Create new carbon footprint calculation
- `GET /history` - Get calculation history (summary data)
- `GET /dashboard-data` - Get full calculation data for dashboard
- `GET /{id}` - Get detailed calculation by ID

### Emission Factors Controller (`/api/emissionfactors`)
- `GET /` - Get all emission factors (with filtering)
- `GET /{id}` - Get specific emission factor
- `POST /` - Create new emission factor

### Mitigation Strategies Controller (`/api/mitigationstrategies`)
- `GET /` - Get all mitigation strategies (with filtering)
- `GET /{id}` - Get specific mitigation strategy

## Switching Between Backends

### Option 1: Mock Backend (Current)
```bash
# Start mock API server
cd carbon-calculator-toolkit-demo
node mock-api-server.js

# Start frontend (points to mock backend)
cd frontend
npm run dev
```

### Option 2: C#/.NET Backend
```bash
# Install .NET 8 SDK
# https://dotnet.microsoft.com/download/dotnet/8.0

# Start .NET API
cd backend/CarbonCalculator.API
dotnet run

# Update frontend config to point to .NET backend
# Edit next.config.ts to use port 5000 instead of 5001
```

## Setup Instructions

### Prerequisites
- **Node.js** 18+ (for mock backend and frontend)
- **.NET 8 SDK** (for C#/.NET backend)
- **MS-SQL Server** or **SQL Server LocalDB** (for production backend)

### Mock Backend Setup (Current)
```bash
# Install dependencies
npm install

# Start mock API server
node mock-api-server.js

# Start frontend (in another terminal)
cd frontend
npm install
npm run dev
```

### C#/.NET Backend Setup
```bash
# Install .NET 8 SDK
# https://dotnet.microsoft.com/download/dotnet/8.0

# Restore packages
cd backend/CarbonCalculator.API
dotnet restore

# Create database
dotnet ef database update

# Run the API
dotnet run

# Update frontend to use .NET backend
# Edit frontend/next.config.ts to point to port 5000
```

## Key C#/.NET Implementation Highlights

### 1. **Domain-Driven Design**
```csharp
public class Calculation
{
    public string CalculationId { get; set; }
    public string TrialId { get; set; }
    public decimal TotalEmissions { get; set; }
    // ... with proper validation attributes
}
```

### 2. **Repository Pattern with Entity Framework**
```csharp
public class CalculationService : ICalculationService
{
    private readonly CarbonCalculatorContext _context;
    // Clean separation of concerns
}
```

### 3. **Dependency Injection**
```csharp
builder.Services.AddScoped<ICalculationService, CalculationService>();
builder.Services.AddDbContext<CarbonCalculatorContext>();
```

### 4. **Proper Error Handling**
```csharp
try
{
    var calculation = await _calculationService.CreateCalculationAsync(request);
    return Ok(calculation);
}
catch (InvalidOperationException ex)
{
    return BadRequest(new { error = ex.Message });
}
```

## Benefits of This Approach

### ✅ **Immediate Demo**
- Mock backend provides instant functionality
- No database setup required for initial demo
- Frontend works immediately

### ✅ **Production Ready**
- C#/.NET backend demonstrates enterprise skills
- Proper database design with Entity Framework
- Clean architecture and best practices
- Scalable and maintainable codebase

### ✅ **Flexibility**
- Easy switching between implementations
- Gradual migration path from mock to production
- Both approaches use identical API contracts

## Next Steps

1. **Install .NET 8 SDK** to run the C#/.NET backend
2. **Set up MS-SQL Server** for production database
3. **Run Entity Framework migrations** to create database schema
4. **Switch frontend configuration** to use .NET backend
5. **Add authentication and authorization** for production use

This hybrid approach demonstrates both **rapid prototyping skills** and **enterprise C#/.NET development expertise**, perfectly suited for the Application Developer role requirements.
