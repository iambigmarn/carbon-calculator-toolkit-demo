using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Infrastructure.Data;

namespace CarbonCalculator.Core.Services;

public class CalculationService : ICalculationService
{
    private readonly CarbonCalculatorContext _context;
    private readonly IEmissionFactorService _emissionFactorService;
    private readonly IMitigationStrategyService _mitigationStrategyService;

    public CalculationService(
        CarbonCalculatorContext context,
        IEmissionFactorService emissionFactorService,
        IMitigationStrategyService mitigationStrategyService)
    {
        _context = context;
        _emissionFactorService = emissionFactorService;
        _mitigationStrategyService = mitigationStrategyService;
    }

    public async Task<Calculation> CreateCalculationAsync(CreateCalculationRequest request)
    {
        var calculationId = $"calc-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
        var activities = new List<CalculationActivity>();
        var totalEmissions = 0m;

        // Process each activity
        foreach (var activityRequest in request.Activities)
        {
            var emissionFactor = await _emissionFactorService.GetEmissionFactorByActivityTypeAndUnitAsync(
                activityRequest.ActivityType, activityRequest.Unit);

            if (emissionFactor == null)
            {
                throw new InvalidOperationException(
                    $"No emission factor found for activity type: {activityRequest.ActivityType} with unit: {activityRequest.Unit}");
            }

            var calculatedEmissions = activityRequest.Quantity * emissionFactor.EmissionFactorValue;
            totalEmissions += calculatedEmissions;

            var activity = new CalculationActivity
            {
                CalculationId = calculationId,
                ActivityType = activityRequest.ActivityType,
                Quantity = activityRequest.Quantity,
                Unit = activityRequest.Unit,
                EmissionFactor = emissionFactor.EmissionFactorValue,
                CalculatedEmissions = calculatedEmissions,
                Description = activityRequest.Description
            };

            activities.Add(activity);
        }

        // Calculate percentages
        foreach (var activity in activities)
        {
            activity.Percentage = totalEmissions > 0 ? (activity.CalculatedEmissions / totalEmissions) * 100 : 0;
        }

        // Identify hotspots
        var hotspots = activities.Select(activity => new CalculationHotspot
        {
            CalculationId = calculationId,
            ActivityType = activity.ActivityType,
            Emissions = activity.CalculatedEmissions,
            Percentage = activity.Percentage,
            Severity = GetSeverityLevel(activity.Percentage),
            Recommendation = GetRecommendation(activity.Percentage, activity.ActivityType)
        }).OrderByDescending(h => h.Percentage).ToList();

        // Get mitigation strategies for high priority hotspots
        var highPriorityHotspots = hotspots.Where(h => h.Severity == "High" || h.Severity == "Critical").ToList();
        var mitigationStrategies = new List<CalculationMitigationStrategy>();

        foreach (var hotspot in highPriorityHotspots)
        {
            var strategies = await _mitigationStrategyService.GetMitigationStrategiesByActivityTypeAsync(hotspot.ActivityType);
            var relevantStrategies = strategies.Take(2);

            foreach (var strategy in relevantStrategies)
            {
                mitigationStrategies.Add(new CalculationMitigationStrategy
                {
                    CalculationId = calculationId,
                    StrategyId = strategy.Id,
                    StrategyName = strategy.StrategyName,
                    Category = strategy.Category,
                    Description = strategy.Description,
                    PotentialReduction = strategy.PotentialReductionPercentage,
                    ImplementationSteps = strategy.ImplementationSteps
                });
            }
        }

        // Remove duplicates
        mitigationStrategies = mitigationStrategies
            .GroupBy(m => m.StrategyId)
            .Select(g => g.First())
            .ToList();

        var calculation = new Calculation
        {
            CalculationId = calculationId,
            TrialId = request.TrialId,
            UserId = request.UserId,
            CalculationName = request.CalculationName ?? $"Calculation {DateTime.UtcNow:yyyy-MM-dd}",
            TotalEmissions = totalEmissions,
            Unit = "kg CO2e",
            CalculationDate = DateTime.UtcNow,
            Status = "Completed"
        };

        _context.Calculations.Add(calculation);
        _context.CalculationActivities.AddRange(activities);
        _context.CalculationHotspots.AddRange(hotspots);
        _context.CalculationMitigationStrategies.AddRange(mitigationStrategies);

        await _context.SaveChangesAsync();

        return calculation;
    }

    public async Task<Calculation?> GetCalculationByIdAsync(string calculationId)
    {
        return await _context.Calculations
            .Include(c => c.Activities)
            .Include(c => c.Hotspots)
            .Include(c => c.MitigationStrategies)
            .FirstOrDefaultAsync(c => c.CalculationId == calculationId);
    }

    public async Task<IEnumerable<Calculation>> GetCalculationsAsync(string? trialId = null, string? userId = null)
    {
        var query = _context.Calculations.AsQueryable();

        if (!string.IsNullOrEmpty(trialId))
            query = query.Where(c => c.TrialId == trialId);

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(c => c.UserId == userId);

        return await query
            .OrderByDescending(c => c.CalculationDate)
            .ToListAsync();
    }

    public async Task<CalculationDetails> GetCalculationDetailsAsync(string calculationId)
    {
        var calculation = await GetCalculationByIdAsync(calculationId);
        if (calculation == null)
            throw new InvalidOperationException($"Calculation with ID {calculationId} not found");

        return new CalculationDetails(
            calculation,
            calculation.Activities,
            calculation.Hotspots
        );
    }

    private static string GetSeverityLevel(decimal percentage)
    {
        return percentage switch
        {
            >= 50 => "Critical",
            >= 25 => "High",
            >= 10 => "Medium",
            _ => "Low"
        };
    }

    private static string GetRecommendation(decimal percentage, string activityType)
    {
        return percentage switch
        {
            >= 50 => $"Immediate action required. {activityType} represents over 50% of total emissions.",
            >= 25 => $"High priority for mitigation. Consider alternative approaches for {activityType}.",
            >= 10 => $"Moderate priority. Review {activityType} for optimization opportunities.",
            _ => $"Low priority. Monitor {activityType} for future optimization."
        };
    }
}
