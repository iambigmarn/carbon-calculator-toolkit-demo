# Carbon Calculator Toolkit - Setup Guide

## Prerequisites

### Backend (.NET)
- .NET 8.0 SDK
- MS-SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Frontend (Next.js)
- Node.js 18+ 
- npm or yarn

## Quick Start

### 1. Backend Setup

```bash
# Navigate to backend directory
cd src/CarbonCalculator.API

# Restore packages
dotnet restore

# Update database (creates database and runs migrations)
dotnet ef database update

# Run the API
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:7001`
- Swagger UI: `https://localhost:7001/swagger`

### 2. Frontend Setup

```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Run the development server
npm run dev
```

The frontend will be available at:
- `http://localhost:3000`

### 3. Database Setup

If you need to set up the database manually:

```sql
-- Run the schema script
-- File: database/schema.sql

-- Run the seed data script
-- File: database/seed-data.sql
```

## Project Structure

```
carbon-calculator-toolkit-demo/
├── src/
│   ├── CarbonCalculator.API/          # ASP.NET Core Web API
│   │   ├── Controllers/               # API Controllers
│   │   ├── Program.cs                 # Application entry point
│   │   └── appsettings.json          # Configuration
│   ├── CarbonCalculator.Core/         # Business logic
│   │   ├── Entities/                  # Domain models
│   │   ├── Services/                  # Business services
│   │   └── Interfaces/                # Service contracts
│   └── CarbonCalculator.Infrastructure/ # Data access
│       └── Data/                      # Entity Framework context
├── frontend/                          # Next.js application
│   ├── src/
│   │   ├── app/                      # Next.js app router
│   │   └── components/                # React components
│   └── package.json
├── database/
│   ├── schema.sql                     # Database schema
│   └── seed-data.sql                  # Sample data
└── docs/                              # Documentation
```

## API Endpoints

### Calculator
- `POST /api/calculator/calculate` - Calculate carbon footprint
- `GET /api/calculator/history` - Get calculation history
- `GET /api/calculator/{id}` - Get calculation details

### Emission Factors
- `GET /api/emissionfactors` - Get all emission factors
- `GET /api/emissionfactors/{id}` - Get specific emission factor
- `POST /api/emissionfactors` - Create new emission factor

### Mitigation Strategies
- `GET /api/mitigationstrategies` - Get mitigation strategies
- `GET /api/mitigationstrategies/{id}` - Get specific strategy

## Features Demonstrated

### Backend (C#/.NET)
- ✅ Clean Architecture with separation of concerns
- ✅ Entity Framework Core with MS-SQL Server
- ✅ RESTful API design with comprehensive error handling
- ✅ Dependency injection and service layer pattern
- ✅ Swagger/OpenAPI documentation
- ✅ Database migrations and seeding

### Frontend (Next.js)
- ✅ Modern React with TypeScript
- ✅ Tailwind CSS for styling
- ✅ Interactive carbon calculator
- ✅ Real-time data visualization with Recharts
- ✅ Responsive design
- ✅ Component-based architecture

### Database Design
- ✅ Normalized schema with proper relationships
- ✅ Performance-optimized indexes
- ✅ Comprehensive emission factors database
- ✅ Calculation history tracking
- ✅ Hotspot identification and mitigation strategies

## Testing the Application

### 1. Carbon Calculator
1. Navigate to the Calculator tab
2. Enter trial information
3. Add activities with quantities and units
4. Click "Calculate Carbon Footprint"
5. View detailed breakdown, hotspots, and mitigation strategies

### 2. Emission Factors
1. Navigate to the Emission Factors tab
2. Browse the comprehensive database
3. Filter by category or search by activity type
4. View detailed emission factor information

### 3. Calculation History
1. Navigate to the History tab
2. View all previous calculations
3. Click on any calculation to see detailed breakdown
4. Analyze hotspots and recommendations

### 4. Dashboard
1. Navigate to the Dashboard tab
2. View comprehensive analytics and visualizations
3. Analyze trends over time
4. Identify top emission sources

## Sample Data

The application comes with realistic sample data including:

- **25 Emission Factors**: Transportation, energy, medical equipment, laboratory, IT, and waste management
- **5 Sample Trials**: Various clinical trial scenarios
- **8 Mitigation Strategies**: Proven carbon reduction approaches
- **Sample Calculations**: Historical carbon footprint assessments

## Troubleshooting

### Backend Issues
- Ensure .NET 8.0 SDK is installed
- Check MS-SQL Server connection string in `appsettings.json`
- Verify Entity Framework migrations are applied

### Frontend Issues
- Ensure Node.js 18+ is installed
- Clear npm cache: `npm cache clean --force`
- Delete `node_modules` and reinstall: `rm -rf node_modules && npm install`

### Database Issues
- Ensure MS-SQL Server is running
- Check connection string configuration
- Run database scripts manually if needed

## Development Notes

This demo showcases enterprise-level development practices:

- **Clean Architecture**: Separation of concerns across layers
- **SOLID Principles**: Single responsibility, dependency inversion
- **API Design**: RESTful endpoints with proper HTTP status codes
- **Error Handling**: Comprehensive error responses and logging
- **Performance**: Database indexing and query optimization
- **Documentation**: Comprehensive API documentation and code comments
- **Testing**: Unit test structure and integration test planning

## Next Steps

To extend this demo:

1. **Add Authentication**: Implement JWT token authentication
2. **Add Validation**: Enhanced input validation and business rules
3. **Add Caching**: Implement Redis caching for emission factors
4. **Add Logging**: Structured logging with Serilog
5. **Add Tests**: Comprehensive unit and integration tests
6. **Add CI/CD**: GitHub Actions for automated deployment

This demo effectively demonstrates the technical skills required for the ICR-CTSU Application Developer position while showcasing understanding of the carbon calculator domain and clinical trial context.
