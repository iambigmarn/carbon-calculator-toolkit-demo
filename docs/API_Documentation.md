# API Documentation - Carbon Calculator Toolkit

## Overview
This document provides comprehensive API documentation for the Carbon Calculator Toolkit demo, showcasing C#/.NET backend development skills for the ICR-CTSU Application Developer position.

## Base URL
```
https://localhost:7001/api
```

## Authentication
Currently, the demo API does not implement authentication. In a production environment, this would include:
- JWT token authentication
- Role-based authorization
- API key management

## API Endpoints

### 1. Carbon Footprint Calculation

#### Calculate Carbon Footprint
**POST** `/calculator/calculate`

Calculates the carbon footprint for a set of activities in a clinical trial.

**Request Body:**
```json
{
  "trialId": "string",
  "userId": "string (optional)",
  "calculationName": "string (optional)",
  "activities": [
    {
      "activityType": "string",
      "quantity": "number",
      "unit": "string",
      "description": "string (optional)"
    }
  ]
}
```

**Response:**
```json
{
  "calculationId": "uuid",
  "trialId": "string",
  "totalEmissions": "number",
  "unit": "string",
  "calculationDate": "datetime",
  "status": "string",
  "breakdown": [
    {
      "activityType": "string",
      "quantity": "number",
      "unit": "string",
      "emissionFactor": "number",
      "calculatedEmissions": "number",
      "percentage": "number"
    }
  ],
  "hotspots": [
    {
      "activityType": "string",
      "emissions": "number",
      "percentage": "number",
      "severity": "string",
      "recommendation": "string"
    }
  ],
  "mitigationStrategies": [
    {
      "strategyId": "number",
      "strategyName": "string",
      "category": "string",
      "description": "string",
      "potentialReduction": "number",
      "implementationSteps": "string"
    }
  ]
}
```

**Example Request:**
```json
{
  "trialId": "TRIAL-001",
  "userId": "user001",
  "calculationName": "Q1 2024 Assessment",
  "activities": [
    {
      "activityType": "Patient Travel",
      "quantity": 1000,
      "unit": "km",
      "description": "Patient visits to clinic"
    },
    {
      "activityType": "Equipment Usage",
      "quantity": 40,
      "unit": "hour",
      "description": "MRI scanner usage"
    }
  ]
}
```

### 2. Emission Factors Management

#### Get All Emission Factors
**GET** `/emission-factors`

Retrieves all active emission factors.

**Query Parameters:**
- `category` (optional): Filter by category (e.g., "Transportation", "Energy")
- `activityType` (optional): Filter by activity type
- `page` (optional): Page number for pagination (default: 1)
- `pageSize` (optional): Items per page (default: 50)

**Response:**
```json
{
  "emissionFactors": [
    {
      "id": "number",
      "category": "string",
      "subCategory": "string",
      "activityType": "string",
      "emissionFactor": "number",
      "unit": "string",
      "description": "string",
      "source": "string",
      "lastUpdated": "datetime"
    }
  ],
  "totalCount": "number",
  "page": "number",
  "pageSize": "number"
}
```

#### Get Emission Factor by ID
**GET** `/emission-factors/{id}`

Retrieves a specific emission factor by ID.

**Response:**
```json
{
  "id": "number",
  "category": "string",
  "subCategory": "string",
  "activityType": "string",
  "emissionFactor": "number",
  "unit": "string",
  "description": "string",
  "source": "string",
  "lastUpdated": "datetime",
  "isActive": "boolean"
}
```

#### Create Emission Factor
**POST** `/emission-factors`

Creates a new emission factor.

**Request Body:**
```json
{
  "category": "string",
  "subCategory": "string",
  "activityType": "string",
  "emissionFactor": "number",
  "unit": "string",
  "description": "string",
  "source": "string"
}
```

### 3. Calculation History

#### Get Calculation History
**GET** `/calculations/history`

Retrieves calculation history with optional filtering.

**Query Parameters:**
- `trialId` (optional): Filter by trial ID
- `userId` (optional): Filter by user ID
- `startDate` (optional): Filter calculations from this date
- `endDate` (optional): Filter calculations to this date
- `page` (optional): Page number
- `pageSize` (optional): Items per page

**Response:**
```json
{
  "calculations": [
    {
      "id": "uuid",
      "trialId": "string",
      "trialName": "string",
      "userId": "string",
      "calculationName": "string",
      "totalEmissions": "number",
      "unit": "string",
      "calculationDate": "datetime",
      "status": "string",
      "activityCount": "number",
      "highestSeverity": "string"
    }
  ],
  "totalCount": "number",
  "page": "number",
  "pageSize": "number"
}
```

#### Get Calculation Details
**GET** `/calculations/{id}`

Retrieves detailed information about a specific calculation.

**Response:**
```json
{
  "calculation": {
    "id": "uuid",
    "trialId": "string",
    "trialName": "string",
    "userId": "string",
    "calculationName": "string",
    "totalEmissions": "number",
    "unit": "string",
    "calculationDate": "datetime",
    "status": "string"
  },
  "activities": [
    {
      "id": "uuid",
      "activityType": "string",
      "quantity": "number",
      "unit": "string",
      "emissionFactor": "number",
      "calculatedEmissions": "number",
      "activityDescription": "string"
    }
  ],
  "hotspots": [
    {
      "id": "number",
      "activityType": "string",
      "emissions": "number",
      "percentageOfTotal": "number",
      "severity": "string",
      "recommendation": "string",
      "mitigationStrategyId": "number"
    }
  ]
}
```

### 4. Mitigation Strategies

#### Get Mitigation Strategies
**GET** `/mitigation-strategies`

Retrieves available mitigation strategies.

**Query Parameters:**
- `category` (optional): Filter by category
- `costCategory` (optional): Filter by cost category (Low, Medium, High)
- `difficulty` (optional): Filter by implementation difficulty (Easy, Medium, Hard)

**Response:**
```json
{
  "strategies": [
    {
      "id": "number",
      "strategyName": "string",
      "category": "string",
      "description": "string",
      "implementationSteps": "string",
      "potentialReductionPercentage": "number",
      "costCategory": "string",
      "implementationDifficulty": "string",
      "applicableActivities": "string"
    }
  ]
}
```

#### Get Mitigation Strategy by ID
**GET** `/mitigation-strategies/{id}`

Retrieves a specific mitigation strategy.

### 5. Trial Management

#### Get Trial Information
**GET** `/trials`

Retrieves trial information.

**Query Parameters:**
- `trialId` (optional): Filter by specific trial ID
- `phase` (optional): Filter by trial phase
- `isActive` (optional): Filter by active status

**Response:**
```json
{
  "trials": [
    {
      "id": "number",
      "trialId": "string",
      "trialName": "string",
      "principalInvestigator": "string",
      "institution": "string",
      "trialPhase": "string",
      "patientCount": "number",
      "durationMonths": "number",
      "description": "string",
      "isActive": "boolean",
      "createdAt": "datetime"
    }
  ]
}
```

#### Create Trial
**POST** `/trials`

Creates a new trial record.

**Request Body:**
```json
{
  "trialId": "string",
  "trialName": "string",
  "principalInvestigator": "string",
  "institution": "string",
  "trialPhase": "string",
  "patientCount": "number",
  "durationMonths": "number",
  "description": "string"
}
```

## Error Handling

All API endpoints return consistent error responses:

**Error Response Format:**
```json
{
  "error": {
    "code": "string",
    "message": "string",
    "details": "string (optional)",
    "timestamp": "datetime",
    "requestId": "string"
  }
}
```

**Common HTTP Status Codes:**
- `200 OK`: Request successful
- `201 Created`: Resource created successfully
- `400 Bad Request`: Invalid request data
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

**Example Error Response:**
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid input data",
    "details": "The 'quantity' field must be greater than 0",
    "timestamp": "2024-01-15T10:30:00Z",
    "requestId": "req-12345"
  }
}
```

## Rate Limiting

The API implements rate limiting to prevent abuse:
- **Rate Limit**: 100 requests per minute per IP
- **Headers**: 
  - `X-RateLimit-Limit`: Maximum requests allowed
  - `X-RateLimit-Remaining`: Remaining requests in current window
  - `X-RateLimit-Reset`: Time when the rate limit resets

## Data Models

### Activity Types
Common activity types used in calculations:
- `Patient Travel`: Patient transportation to/from clinic
- `Staff Commuting`: Staff transportation to/from work
- `Equipment Usage`: Medical equipment energy consumption
- `Building Operations`: Facility energy usage
- `Laboratory Operations`: Lab equipment and chemical usage
- `Data Storage`: IT infrastructure energy consumption

### Severity Levels
Hotspot severity classifications:
- `Low`: < 10% of total emissions
- `Medium`: 10-25% of total emissions
- `High`: 25-50% of total emissions
- `Critical`: > 50% of total emissions

### Units
Supported measurement units:
- Distance: `km`, `miles`
- Energy: `kWh`, `MJ`
- Time: `hour`, `day`, `month`
- Weight: `kg`, `tonnes`
- Volume: `liter`, `m3`

## Testing the API

### Using Swagger UI
1. Start the application
2. Navigate to `https://localhost:7001/swagger`
3. Use the interactive interface to test endpoints

### Using cURL Examples

**Calculate Carbon Footprint:**
```bash
curl -X POST "https://localhost:7001/api/calculator/calculate" \
  -H "Content-Type: application/json" \
  -d '{
    "trialId": "TRIAL-001",
    "activities": [
      {
        "activityType": "Patient Travel",
        "quantity": 500,
        "unit": "km"
      }
    ]
  }'
```

**Get Emission Factors:**
```bash
curl -X GET "https://localhost:7001/api/emission-factors?category=Transportation"
```

**Get Calculation History:**
```bash
curl -X GET "https://localhost:7001/api/calculations/history?trialId=TRIAL-001"
```

## Performance Considerations

- **Database Indexing**: Optimized indexes on frequently queried columns
- **Caching**: Emission factors cached for improved performance
- **Pagination**: Large result sets paginated to improve response times
- **Async Operations**: Database operations use async/await patterns
- **Connection Pooling**: Efficient database connection management

This API demonstrates modern C#/.NET development practices including clean architecture, comprehensive error handling, and performance optimization techniques relevant to the ICR-CTSU Application Developer position.
