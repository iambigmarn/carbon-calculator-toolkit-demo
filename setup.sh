#!/bin/bash

# Carbon Calculator Toolkit - Setup Script
# This script helps set up the development environment

echo "🌱 Carbon Calculator Toolkit Setup"
echo "=================================="

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Please install .NET 8.0 SDK first."
    echo "   Visit: https://dotnet.microsoft.com/download"
    exit 1
fi

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "❌ Node.js not found. Please install Node.js 18+ first."
    echo "   Visit: https://nodejs.org/"
    exit 1
fi

echo "✅ Prerequisites check passed"
echo ""

# Setup Backend
echo "🔧 Setting up Backend (ASP.NET Core)..."
cd src/CarbonCalculator.API

echo "📦 Restoring NuGet packages..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "❌ Failed to restore packages"
    exit 1
fi

echo "🗄️ Setting up database..."
echo "Note: This will create the database and run migrations"
echo "Make sure MS-SQL Server is running and accessible"

# Check if Entity Framework tools are available
if ! dotnet ef --version &> /dev/null; then
    echo "📦 Installing Entity Framework tools..."
    dotnet tool install --global dotnet-ef
fi

echo "🔄 Running database migrations..."
dotnet ef database update

if [ $? -ne 0 ]; then
    echo "⚠️ Database update failed. You may need to set up the database manually."
    echo "   Run the scripts in the database/ folder"
fi

echo "✅ Backend setup complete"
echo ""

# Setup Frontend
echo "🔧 Setting up Frontend (Next.js)..."
cd ../../frontend

echo "📦 Installing npm packages..."
npm install

if [ $? -ne 0 ]; then
    echo "❌ Failed to install npm packages"
    exit 1
fi

echo "✅ Frontend setup complete"
echo ""

# Final instructions
echo "🎉 Setup Complete!"
echo ""
echo "To run the application:"
echo ""
echo "Backend:"
echo "  cd src/CarbonCalculator.API"
echo "  dotnet run"
echo "  API will be available at: https://localhost:7001"
echo "  Swagger UI: https://localhost:7001/swagger"
echo ""
echo "Frontend:"
echo "  cd frontend"
echo "  npm run dev"
echo "  Frontend will be available at: http://localhost:3000"
echo ""
echo "📚 For detailed instructions, see SETUP_GUIDE.md"
echo ""
echo "🚀 Happy coding!"
