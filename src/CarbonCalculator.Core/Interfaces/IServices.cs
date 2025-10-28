using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.Core.Interfaces
{
    public interface ICalculationService
    {
        Task<CalculationResult> CalculateCarbonFootprintAsync(CalculationRequest request);
        Task<IEnumerable<CalculationHistory>> GetCalculationHistoryAsync(string? trialId = null, string? userId = null);
        Task<CalculationDetails?> GetCalculationDetailsAsync(Guid calculationId);
    }

    public interface IEmissionFactorService
    {
        Task<IEnumerable<EmissionFactor>> GetEmissionFactorsAsync(string? category = null, string? activityType = null);
        Task<EmissionFactor?> GetEmissionFactorByIdAsync(int id);
        Task<EmissionFactor> CreateEmissionFactorAsync(CreateEmissionFactorRequest request);
        Task<EmissionFactor?> GetEmissionFactorAsync(string activityType, string unit);
    }

    public interface IMitigationStrategyService
    {
        Task<IEnumerable<MitigationStrategy>> GetMitigationStrategiesAsync(string? category = null, string? costCategory = null, string? difficulty = null);
        Task<MitigationStrategy?> GetMitigationStrategyByIdAsync(int id);
    }

    // DTOs
    public class CalculationRequest
    {
        public string TrialId { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? CalculationName { get; set; }
        public List<ActivityRequest> Activities { get; set; } = new List<ActivityRequest>();
    }

    public class ActivityRequest
    {
        public string ActivityType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CalculationResult
    {
        public Guid CalculationId { get; set; }
        public string TrialId { get; set; } = string.Empty;
        public decimal TotalEmissions { get; set; }
        public string Unit { get; set; } = "kg CO2e";
        public DateTime CalculationDate { get; set; }
        public string Status { get; set; } = "Completed";
        public List<ActivityBreakdown> Breakdown { get; set; } = new List<ActivityBreakdown>();
        public List<HotspotInfo> Hotspots { get; set; } = new List<HotspotInfo>();
        public List<MitigationStrategyInfo> MitigationStrategies { get; set; } = new List<MitigationStrategyInfo>();
    }

    public class ActivityBreakdown
    {
        public string ActivityType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal EmissionFactor { get; set; }
        public decimal CalculatedEmissions { get; set; }
        public decimal Percentage { get; set; }
    }

    public class HotspotInfo
    {
        public string ActivityType { get; set; } = string.Empty;
        public decimal Emissions { get; set; }
        public decimal Percentage { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string? Recommendation { get; set; }
    }

    public class MitigationStrategyInfo
    {
        public int StrategyId { get; set; }
        public string StrategyName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? PotentialReduction { get; set; }
        public string? ImplementationSteps { get; set; }
    }

    public class CalculationDetails
    {
        public CalculationHistory Calculation { get; set; } = null!;
        public List<CalculationActivity> Activities { get; set; } = new List<CalculationActivity>();
        public List<Hotspot> Hotspots { get; set; } = new List<Hotspot>();
    }

    public class CreateEmissionFactorRequest
    {
        public string Category { get; set; } = string.Empty;
        public string SubCategory { get; set; } = string.Empty;
        public string ActivityType { get; set; } = string.Empty;
        public decimal EmissionFactorValue { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Source { get; set; }
    }
}
