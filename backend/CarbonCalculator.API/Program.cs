using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://127.0.0.1:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/api/health", () => new { status = "OK", timestamp = DateTime.UtcNow, backend = "C#/.NET Core" });

// Simple calculator endpoint for demo
app.MapPost("/api/calculator/calculate", ([FromBody] CalculationRequest request) =>
{
    var calculationId = $"calc-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
    
    // Simple calculation logic (matching the mock API)
    var breakdown = request.Activities.Select(activity => new
    {
        activityType = activity.ActivityType,
        quantity = activity.Quantity,
        unit = activity.Unit,
        emissionFactor = GetEmissionFactor(activity.ActivityType, activity.Unit),
        calculatedEmissions = activity.Quantity * GetEmissionFactor(activity.ActivityType, activity.Unit),
        percentage = 0.0m // Will be calculated below
    }).ToList();

    var totalEmissions = breakdown.Sum(b => b.calculatedEmissions);
    
    // Calculate percentages
    var breakdownWithPercentages = breakdown.Select(item => new
    {
        item.activityType,
        item.quantity,
        item.unit,
        item.emissionFactor,
        item.calculatedEmissions,
        percentage = totalEmissions > 0 ? (item.calculatedEmissions / totalEmissions) * 100 : 0
    }).ToList();

    // Identify hotspots
    var hotspots = breakdownWithPercentages.Select(item => new
    {
        activityType = item.activityType,
        emissions = item.calculatedEmissions,
        percentage = item.percentage,
        severity = GetSeverityLevel(item.percentage),
        recommendation = GetRecommendation(item.percentage, item.activityType)
    }).OrderByDescending(h => h.percentage).ToList();

    var result = new
    {
        calculationId,
        trialId = request.TrialId,
        userId = request.UserId,
        calculationName = request.CalculationName ?? $"Calculation {DateTime.UtcNow:yyyy-MM-dd}",
        totalEmissions,
        unit = "kg CO2e",
        calculationDate = DateTime.UtcNow,
        status = "Completed",
        breakdown = breakdownWithPercentages,
        hotspots,
        mitigationStrategies = new object[0] // Empty for now
    };

    return Results.Ok(result);
});

// Simple history endpoint
app.MapGet("/api/calculator/history", () =>
{
    // Return empty array for now - in real implementation this would come from database
    return Results.Ok(new object[0]);
});

// Simple dashboard data endpoint
app.MapGet("/api/calculator/dashboard-data", () =>
{
    // Return empty array for now - in real implementation this would come from database
    return Results.Ok(new object[0]);
});

app.Run();

// Helper methods
static decimal GetEmissionFactor(string activityType, string unit)
{
    // Simple emission factors (matching the mock API)
    return activityType switch
    {
        "Patient Travel" when unit == "km" => 0.192m,
        "Equipment Usage" when unit == "hour" => 15.0m,
        "Staff Commuting" when unit == "km" => 0.192m,
        "Building Operations" when unit == "kWh" => 0.233m,
        _ => 1.0m // Default fallback
    };
}

static string GetSeverityLevel(decimal percentage)
{
    return percentage switch
    {
        >= 50 => "Critical",
        >= 25 => "High",
        >= 10 => "Medium",
        _ => "Low"
    };
}

static string GetRecommendation(decimal percentage, string activityType)
{
    return percentage switch
    {
        >= 50 => $"Immediate action required. {activityType} represents over 50% of total emissions.",
        >= 25 => $"High priority for mitigation. Consider alternative approaches for {activityType}.",
        >= 10 => $"Moderate priority. Review {activityType} for optimization opportunities.",
        _ => $"Low priority. Monitor {activityType} for future optimization."
    };
}

// DTOs
public record CalculationRequest(
    string TrialId,
    string UserId,
    string? CalculationName,
    IEnumerable<ActivityRequest> Activities
);

public record ActivityRequest(
    string ActivityType,
    decimal Quantity,
    string Unit,
    string? Description
);