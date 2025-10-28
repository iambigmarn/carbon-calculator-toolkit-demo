using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.Core.Interfaces;

public interface ICalculationService
{
    Task<Calculation> CreateCalculationAsync(CreateCalculationRequest request);
    Task<Calculation?> GetCalculationByIdAsync(string calculationId);
    Task<IEnumerable<Calculation>> GetCalculationsAsync(string? trialId = null, string? userId = null);
    Task<CalculationDetails> GetCalculationDetailsAsync(string calculationId);
}

public interface IEmissionFactorService
{
    Task<IEnumerable<EmissionFactor>> GetEmissionFactorsAsync(string? category = null, string? activityType = null);
    Task<EmissionFactor?> GetEmissionFactorByIdAsync(int id);
    Task<EmissionFactor> CreateEmissionFactorAsync(CreateEmissionFactorRequest request);
}

public interface IMitigationStrategyService
{
    Task<IEnumerable<MitigationStrategy>> GetMitigationStrategiesAsync(string? category = null, string? costCategory = null, string? difficulty = null);
    Task<MitigationStrategy?> GetMitigationStrategyByIdAsync(int id);
}

// DTOs
public record CreateCalculationRequest(
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

public record CalculationDetails(
    Calculation Calculation,
    IEnumerable<CalculationActivity> Activities,
    IEnumerable<CalculationHotspot> Hotspots
);

public record CreateEmissionFactorRequest(
    string Category,
    string SubCategory,
    string ActivityType,
    decimal EmissionFactorValue,
    string Unit,
    string? Description,
    string? Source
);
