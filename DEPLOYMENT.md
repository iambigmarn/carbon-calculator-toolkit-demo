# 🚀 Deployment Guide

This guide covers multiple deployment options for the Carbon Calculator Toolkit Demo.

## Option 1: Vercel (Recommended for Frontend)

### Frontend Deployment to Vercel

1. **Install Vercel CLI:**
```bash
npm install -g vercel
```

2. **Deploy from frontend directory:**
```bash
cd frontend
vercel
```

3. **Configure environment variables in Vercel dashboard:**
```
NEXT_PUBLIC_API_URL=https://your-backend-url.com
```

4. **Update API configuration:**
```typescript
// frontend/next.config.ts
const nextConfig = {
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'https://your-backend-url.com/api/:path*',
      },
    ]
  },
}
```

## Option 2: Railway (Backend)

### C#/.NET API Deployment to Railway

1. **Connect GitHub repository to Railway**
2. **Configure build settings:**
   - Build Command: `dotnet publish -c Release`
   - Start Command: `dotnet CarbonCalculator.API.dll`
   - Port: `5000`

3. **Add environment variables:**
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:5000
```

## Option 3: GitHub Pages (Static)

### Static Frontend Deployment

1. **Build static version:**
```bash
cd frontend
npm run build
npm run export
```

2. **Enable GitHub Pages:**
   - Go to repository Settings
   - Scroll to "Pages" section
   - Select "Deploy from a branch"
   - Choose `gh-pages` branch

3. **Create deployment script:**
```bash
# deploy.sh
#!/bin/bash
cd frontend
npm run build
npm run export
git add out/
git commit -m "Deploy to GitHub Pages"
git subtree push --prefix out origin gh-pages
```

## Option 4: Docker Deployment

### Create Dockerfile for Frontend

```dockerfile
# frontend/Dockerfile
FROM node:18-alpine

WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production

COPY . .
RUN npm run build

EXPOSE 3000
CMD ["npm", "start"]
```

### Create Dockerfile for Backend

```dockerfile
# backend/CarbonCalculator.API/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CarbonCalculator.API/CarbonCalculator.API.csproj", "CarbonCalculator.API/"]
RUN dotnet restore "CarbonCalculator.API/CarbonCalculator.API.csproj"
COPY . .
WORKDIR "/src/CarbonCalculator.API"
RUN dotnet build "CarbonCalculator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarbonCalculator.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarbonCalculator.API.dll"]
```

### Docker Compose

```yaml
# docker-compose.yml
version: '3.8'
services:
  frontend:
    build: ./frontend
    ports:
      - "3000:3000"
    environment:
      - NEXT_PUBLIC_API_URL=http://backend:5000
  
  backend:
    build: ./backend/CarbonCalculator.API
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
```

## Option 5: Netlify (Frontend)

### Deploy to Netlify

1. **Connect GitHub repository to Netlify**
2. **Configure build settings:**
   - Build Command: `cd frontend && npm run build`
   - Publish Directory: `frontend/out`
   - Base Directory: `frontend`

3. **Add redirects for SPA:**
```toml
# _redirects
/api/* https://your-backend-url.com/api/:splat 200
/* /index.html 200
```

## Environment Variables

### Frontend (.env.local)
```
NEXT_PUBLIC_API_URL=http://localhost:5001
NEXT_PUBLIC_APP_NAME=Carbon Calculator Toolkit
```

### Backend (appsettings.Production.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=CarbonCalculatorDb;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

## Production Checklist

- [ ] Update API URLs for production
- [ ] Configure CORS for production domains
- [ ] Set up database connection strings
- [ ] Enable HTTPS/SSL certificates
- [ ] Configure logging and monitoring
- [ ] Set up error tracking (Sentry, etc.)
- [ ] Configure CDN for static assets
- [ ] Set up automated backups
- [ ] Configure health checks
- [ ] Set up performance monitoring

## Monitoring & Analytics

### Frontend Monitoring
```typescript
// Add to _app.tsx
import { Analytics } from '@vercel/analytics/react'

export default function App({ Component, pageProps }) {
  return (
    <>
      <Component {...pageProps} />
      <Analytics />
    </>
  )
}
```

### Backend Health Checks
```csharp
// Add to Program.cs
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

## Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure CORS is configured for production domains
2. **API Connection**: Verify API URLs are correct in production
3. **Build Failures**: Check Node.js and .NET versions match requirements
4. **Database Connection**: Verify connection strings and database accessibility

### Debug Commands

```bash
# Check frontend build
cd frontend && npm run build

# Check backend build
cd backend/CarbonCalculator.API && dotnet build

# Test API endpoints
curl -X GET https://your-api-url.com/api/health
```

---

**Ready to deploy!** Choose the option that best fits your needs and follow the steps above.
