# Carbon Calculator Toolkit - Demo Plan

## Project Overview
**Project Name:** Carbon Calculator Toolkit Demo  
**Target:** ICR-CTSU Application Developer Position  
**Focus:** C#/.NET Backend, MS-SQL Database, API Development  

## Demo Objectives
Demonstrate proficiency in:
- C#/.NET backend development
- MS-SQL database design and implementation
- RESTful API development
- Carbon footprint calculation algorithms
- Data modeling for emission factors
- Web application architecture planning

## Core Features to Demonstrate

### 1. Carbon Footprint Calculator Engine
- **Emission Factor Database**: MS-SQL database storing emission factors for various activities
- **Calculation Engine**: C# service layer performing carbon footprint calculations
- **API Endpoints**: RESTful APIs for calculation requests and results

### 2. Database Schema Design
- **EmissionFactors Table**: Store carbon emission factors by activity type
- **CalculationHistory Table**: Track user calculations and results
- **MitigationStrategies Table**: Store advice for carbon reduction
- **Hotspots Table**: Identify high-emission areas in trials

### 3. Backend API Architecture
- **ASP.NET Core Web API** with clean architecture
- **Entity Framework Core** for database operations
- **Dependency Injection** for service management
- **Swagger/OpenAPI** documentation
- **Logging and Error Handling**

## Technical Implementation Plan

### Phase 1: Database Design & Setup
```
1. Design MS-SQL database schema
2. Create Entity Framework models
3. Set up database migrations
4. Seed initial emission factor data
```

### Phase 2: Core API Development
```
1. Create ASP.NET Core Web API project
2. Implement calculation service layer
3. Build RESTful endpoints:
   - POST /api/calculator/calculate
   - GET /api/emission-factors
   - GET /api/mitigation-strategies
   - GET /api/history/{userId}
```

### Phase 3: Business Logic Implementation
```
1. Carbon calculation algorithms
2. Hotspot identification logic
3. Mitigation strategy recommendations
4. Data validation and error handling
```

### Phase 4: Documentation & Testing
```
1. API documentation with Swagger
2. Unit tests for calculation engine
3. Integration tests for API endpoints
4. Database performance optimization
```

## Sample Data & Use Cases

### Clinical Trial Carbon Footprint Scenarios
1. **Patient Travel**: Calculate emissions from patient visits
2. **Medical Equipment**: Energy consumption of medical devices
3. **Laboratory Work**: Chemical usage and waste disposal
4. **Data Storage**: Server and cloud computing emissions
5. **Staff Commuting**: Transportation emissions

### Emission Factors Database
- Transportation: Car (0.192 kg CO2/km), Train (0.041 kg CO2/km)
- Energy: Electricity (0.233 kg CO2/kWh), Natural Gas (0.202 kg CO2/kWh)
- Medical Equipment: MRI (15 kWh/hour), CT Scanner (8 kWh/hour)
- Laboratory: Chemical disposal (varies by chemical type)

## API Endpoints Specification

### Calculation API
```http
POST /api/calculator/calculate
Content-Type: application/json

{
  "trialId": "TRIAL-001",
  "activities": [
    {
      "type": "patient_travel",
      "distance": 50,
      "transportMode": "car",
      "frequency": 12
    },
    {
      "type": "equipment_usage",
      "equipmentType": "mri",
      "hours": 8,
      "frequency": 1
    }
  ]
}
```

### Response Format
```json
{
  "calculationId": "CALC-12345",
  "totalEmissions": 125.6,
  "unit": "kg CO2e",
  "breakdown": [
    {
      "activity": "patient_travel",
      "emissions": 115.2,
      "percentage": 91.7
    },
    {
      "activity": "equipment_usage",
      "emissions": 10.4,
      "percentage": 8.3
    }
  ],
  "hotspots": [
    {
      "activity": "patient_travel",
      "severity": "high",
      "recommendation": "Consider virtual consultations for follow-up visits"
    }
  ],
  "mitigationStrategies": [
    {
      "strategy": "Virtual Consultations",
      "potentialReduction": 60,
      "implementation": "Use telemedicine for routine follow-ups"
    }
  ]
}
```

## Project Structure
```
carbon-calculator-toolkit-demo/
├── src/
│   ├── CarbonCalculator.API/          # ASP.NET Core Web API
│   ├── CarbonCalculator.Core/         # Business logic and models
│   ├── CarbonCalculator.Infrastructure/ # Data access layer
│   └── CarbonCalculator.Tests/        # Unit and integration tests
├── database/
│   ├── schema.sql                     # Database schema
│   ├── seed-data.sql                  # Initial data
│   └── migrations/                    # EF Core migrations
├── docs/
│   ├── API_Documentation.md
│   ├── Database_Design.md
│   └── Deployment_Guide.md
└── README.md
```

## Key Technical Highlights

### 1. Clean Architecture Implementation
- Separation of concerns with Core, Infrastructure, and API layers
- Dependency injection for testability
- Repository pattern for data access

### 2. Database Design Excellence
- Normalized schema for emission factors
- Efficient indexing for calculation queries
- Audit trails for calculation history

### 3. API Design Best Practices
- RESTful endpoint design
- Comprehensive error handling
- Input validation and sanitization
- Rate limiting considerations

### 4. Performance Considerations
- Caching strategies for emission factors
- Async/await patterns for database operations
- Connection pooling and optimization

## Demo Presentation Flow

### 1. Architecture Overview (5 minutes)
- Show project structure and clean architecture
- Explain database design and relationships
- Highlight separation of concerns

### 2. Database Demonstration (5 minutes)
- Display MS-SQL database schema
- Show emission factors data
- Demonstrate query performance

### 3. API Testing (10 minutes)
- Live API testing with Postman/Swagger
- Show calculation endpoints
- Demonstrate error handling

### 4. Code Walkthrough (10 minutes)
- Explain calculation algorithms
- Show service layer implementation
- Highlight testing strategies

### 5. Future Enhancements (5 minutes)
- Discuss web application integration
- Outline scalability considerations
- Present additional features roadmap

## Success Metrics
- ✅ Functional carbon calculation engine
- ✅ Well-designed MS-SQL database
- ✅ RESTful API with proper documentation
- ✅ Clean, maintainable C# code
- ✅ Comprehensive test coverage
- ✅ Performance optimization examples

## Timeline Estimate
- **Setup & Database Design**: 2-3 hours
- **Core API Development**: 4-5 hours
- **Business Logic Implementation**: 3-4 hours
- **Testing & Documentation**: 2-3 hours
- **Total Development Time**: 11-15 hours

This demo will effectively showcase the technical skills required for the ICR-CTSU position while demonstrating understanding of the carbon calculator domain and clinical trial context.
