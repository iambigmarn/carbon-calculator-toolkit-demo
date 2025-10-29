# 🌱 Carbon Calculator Toolkit Demo

> **A comprehensive carbon footprint calculator for clinical trials with hybrid backend architecture**

[![Next.js](https://img.shields.io/badge/Next.js-14.0-black)](https://nextjs.org/)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.0-blue)](https://www.typescriptlang.org/)
[![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-3.0-38B2AC)](https://tailwindcss.com/)

## 🎯 Project Overview

This project demonstrates a **Carbon Calculator Toolkit** designed for publicly funded clinical trials. It showcases both rapid prototyping capabilities and enterprise-grade development skills through a hybrid backend architecture.

### Key Features
- ✅ **Real-time Carbon Footprint Calculation** for clinical trial activities
- ✅ **Interactive Dashboard** with data visualization (charts, graphs, trends)
- ✅ **Emission Factors Database** with searchable, categorized factors
- ✅ **Calculation History** with detailed breakdowns and hotspot analysis
- ✅ **Mitigation Strategy Recommendations** based on emission hotspots
- ✅ **Mobile-Responsive Design** optimized for all devices
- ✅ **Hybrid Backend Architecture** (Node.js mock + C#/.NET Core)

## 🏗️ Architecture

### Frontend
- **Next.js 14** with TypeScript
- **Tailwind CSS** for responsive design
- **Recharts** for data visualization
- **React Hooks** for state management

### Backend Options
1. **Mock API Server** (Node.js/Express) - Port 5001
2. **C#/.NET Core Web API** - Port 5002

### Database Design
- **MS-SQL Server** schema with 6 normalized tables
- **Entity Framework Core** for data access
- **Stored procedures** and **views** for complex queries

## 🚀 Quick Start

### Prerequisites
- Node.js 18+ 
- .NET 9.0 SDK (optional, for C# backend)
- Git

### Option 1: Mock Backend (Recommended for Demo)

```bash
# Clone the repository
git clone <your-github-repo-url>
cd carbon-calculator-toolkit-demo

# Install dependencies
npm install

# Start mock API server
node mock-api-server.js

# In a new terminal, start the frontend
cd frontend
npm install
npm run dev
```

**Access the application:** http://localhost:3000

### Option 2: C#/.NET Backend

```bash
# Install .NET 9.0 SDK
# macOS: brew install dotnet
# Windows: Download from https://dotnet.microsoft.com/download

# Start C#/.NET API
cd backend/CarbonCalculator.API
dotnet restore
dotnet run --urls "http://localhost:5002"

# Update frontend config to use port 5002
# Edit frontend/next.config.ts and change port from 5001 to 5002

# Start frontend
cd frontend
npm install
npm run dev
```

## 📱 Demo Walkthrough

### 1. **Carbon Calculator**
- Navigate to the "Calculator" tab
- Fill in trial information (Trial ID, User ID, Calculation Name)
- Add activities with quantities and units
- Click "Calculate Carbon Footprint"
- View detailed breakdown and hotspot analysis

### 2. **Dashboard**
- Switch to "Dashboard" tab
- View summary statistics and trends
- Explore interactive charts and graphs
- Analyze emission patterns over time

### 3. **Emission Factors**
- Visit "Emission Factors" tab
- Browse categorized emission factors
- Search and filter by category
- View detailed factor information

### 4. **Calculation History**
- Check "History" tab
- View all previous calculations
- Click "View Details" for detailed breakdowns
- Analyze mitigation recommendations

## 🛠️ Technical Implementation

### API Endpoints

#### Mock API (Port 5001)
```bash
GET  /api/health                    # Health check
POST /api/calculator/calculate       # Calculate carbon footprint
GET  /api/calculator/history        # Get calculation history
GET  /api/calculator/dashboard-data # Get dashboard data
GET  /api/emission-factors          # Get emission factors
```

#### C#/.NET API (Port 5002)
```bash
GET  /api/health                    # Health check
POST /api/calculator/calculate       # Calculate carbon footprint
GET  /api/calculator/history        # Get calculation history
GET  /api/calculator/dashboard-data # Get dashboard data
```

### Database Schema

```sql
-- Core Tables
Trials (TrialId, TrialName, Description, StartDate, EndDate)
Users (UserId, Username, Email, Role)
EmissionFactors (FactorId, ActivityType, Unit, Value, Category)
MitigationStrategies (StrategyId, Name, Description, ImpactLevel)
Calculations (CalculationId, TrialId, UserId, CalculationName, TotalEmissions)
CalculationActivities (ActivityId, CalculationId, ActivityType, Quantity, Unit)
```

### Key Technologies Demonstrated

#### Frontend Development
- **React/Next.js**: Component-based architecture
- **TypeScript**: Type safety and better development experience
- **Tailwind CSS**: Utility-first CSS framework
- **Recharts**: Data visualization library
- **Responsive Design**: Mobile-first approach

#### Backend Development
- **C#/.NET Core**: Enterprise-grade web API
- **Entity Framework Core**: ORM for database operations
- **Clean Architecture**: Separation of concerns
- **Dependency Injection**: SOLID principles
- **RESTful API Design**: Standard HTTP methods and status codes

#### Database Design
- **MS-SQL Server**: Relational database
- **Normalized Schema**: 3NF database design
- **Indexes**: Performance optimization
- **Stored Procedures**: Complex business logic
- **Views**: Data abstraction

## 📊 Sample Data

The application includes sample emission factors for:
- **Patient Travel**: 0.192 kg CO2e/km
- **Equipment Usage**: 15.0 kg CO2e/hour
- **Staff Commuting**: 0.192 kg CO2e/km
- **Building Operations**: 0.233 kg CO2e/kWh

## 🔧 Configuration

### Switching Backends

To switch from mock API to C#/.NET API:

1. **Update frontend configuration:**
```typescript
// frontend/next.config.ts
rewrites: async () => [
  {
    source: '/api/:path*',
    destination: 'http://localhost:5002/api/:path*', // Change from 5001 to 5002
  },
]
```

2. **Restart the frontend:**
```bash
cd frontend
npm run dev
```

## 📈 Performance Features

- **Lazy Loading**: Components loaded on demand
- **Data Caching**: API responses cached for better performance
- **Responsive Images**: Optimized for different screen sizes
- **Code Splitting**: Reduced initial bundle size
- **Database Indexing**: Optimized query performance

## 🧪 Testing

```bash
# Run frontend tests
cd frontend
npm test

# Run API tests (C#/.NET)
cd backend/CarbonCalculator.API
dotnet test
```

## 🚀 Deployment Options

### Option 1: Vercel (Frontend) + Railway (Backend)
```bash
# Deploy frontend to Vercel
npm install -g vercel
cd frontend
vercel

# Deploy C#/.NET API to Railway
# Connect GitHub repo to Railway
# Configure build command: dotnet publish
```

### Option 2: GitHub Pages (Static)
```bash
# Build static version
cd frontend
npm run build
npm run export

# Deploy to GitHub Pages
# Enable GitHub Pages in repository settings
```

## 📝 Development Notes

### Project Structure
```
carbon-calculator-toolkit-demo/
├── frontend/                 # Next.js frontend
│   ├── src/
│   │   ├── app/            # App router pages
│   │   └── components/     # React components
│   ├── package.json
│   └── next.config.ts
├── backend/                 # C#/.NET backend
│   ├── CarbonCalculator.API/
│   ├── CarbonCalculator.Core/
│   └── CarbonCalculator.Infrastructure/
├── database/               # SQL schema and seed data
├── docs/                   # Documentation
├── mock-api-server.js      # Node.js mock API
└── README.md
```

### Key Design Decisions

1. **Hybrid Backend**: Demonstrates both rapid prototyping (Node.js) and enterprise development (C#/.NET)
2. **Mobile-First**: Responsive design for all device types
3. **Type Safety**: TypeScript throughout the application
4. **Clean Architecture**: Separation of concerns in C# backend
5. **RESTful APIs**: Standard HTTP methods and status codes

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

**Developer**: Ibe Chimaraoke Emmanuel
**Email**: emma.zgtc@gmail.com
**LinkedIn**: https://ng.linkedin.com/in/ibe-chimaraoke-b44340374

---

## 🎯 For Employers

This project demonstrates:

✅ **Full-Stack Development**: Frontend (React/Next.js) + Backend (C#/.NET)  
✅ **Database Design**: MS-SQL Server with Entity Framework Core  
✅ **API Development**: RESTful services with proper error handling  
✅ **Modern Frontend**: TypeScript, responsive design, data visualization  
✅ **Clean Architecture**: SOLID principles and separation of concerns  
✅ **DevOps Awareness**: Git, deployment strategies, environment configuration  
✅ **Documentation**: Comprehensive README and code documentation  

**Perfect for**: Application Developer roles requiring C#/.NET, MS-SQL, and modern web development skills.

---

*Built with ❤️ for the Carbon Calculator Toolkit position*