using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Infrastructure.Data;

namespace CarbonCalculator.Core.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly CarbonCalculatorContext _context;
        private readonly IEmissionFactorService _emissionFactorService;

        public CalculationService(CarbonCalculatorContext context, IEmissionFactorService emissionFactorService)
        {
            _context = context;
            _emissionFactorService = emissionFactorService;
        }

        public async Task<CalculationResult> CalculateCarbonFootprintAsync(CalculationRequest request)
        {
            var calculationId = Guid.NewGuid();
            var totalEmissions = 0m;
            var breakdown = new List<ActivityBreakdown>();
            var hotspots = new List<HotspotInfo>();

            // Create calculation history record
            var calculation = new CalculationHistory
            {
                Id = calculationId,
                TrialId = request.TrialId,
                UserId = request.UserId,
                CalculationName = request.CalculationName,
                TotalEmissions = 0,
                Status = "InProgress"
            };

            _context.CalculationHistories.Add(calculation);

            try
            {
                // Process each activity
                foreach (var activity in request.Activities)
                {
                    var emissionFactor = await _emissionFactorService.GetEmissionFactorAsync(activity.ActivityType, activity.Unit);
                    
                    if (emissionFactor == null)
                    {
                        throw new InvalidOperationException($"No emission factor found for activity type: {activity.ActivityType} with unit: {activity.Unit}");
                    }

                    var calculatedEmissions = activity.Quantity * emissionFactor.EmissionFactorValue;
                    totalEmissions += calculatedEmissions;

                    // Create activity record
                    var activityRecord = new CalculationActivity
                    {
                        Id = Guid.NewGuid(),
                        CalculationId = calculationId,
                        ActivityType = activity.ActivityType,
                        Quantity = activity.Quantity,
                        Unit = activity.Unit,
                        EmissionFactor = emissionFactor.EmissionFactorValue,
                        CalculatedEmissions = calculatedEmissions,
                        ActivityDescription = activity.Description
                    };

                    _context.CalculationActivities.Add(activityRecord);

                    // Add to breakdown
                    breakdown.Add(new ActivityBreakdown
                    {
                        ActivityType = activity.ActivityType,
                        Quantity = activity.Quantity,
                        Unit = activity.Unit,
                        EmissionFactor = emissionFactor.EmissionFactorValue,
                        CalculatedEmissions = calculatedEmissions,
                        Percentage = 0 // Will be calculated after total is known
                    });
                }

                // Calculate percentages
                foreach (var item in breakdown)
                {
                    item.Percentage = totalEmissions > 0 ? (item.CalculatedEmissions / totalEmissions) * 100 : 0;
                }

                // Identify hotspots
                hotspots = IdentifyHotspots(breakdown);

                // Update calculation with total
                calculation.TotalEmissions = totalEmissions;
                calculation.Status = "Completed";

                // Save hotspots to database
                foreach (var hotspot in hotspots)
                {
                    var hotspotRecord = new Hotspot
                    {
                        CalculationId = calculationId,
                        ActivityType = hotspot.ActivityType,
                        Emissions = hotspot.Emissions,
                        PercentageOfTotal = hotspot.Percentage,
                        Severity = hotspot.Severity,
                        Recommendation = hotspot.Recommendation
                    };
                    _context.Hotspots.Add(hotspotRecord);
                }

                await _context.SaveChangesAsync();

                // Get mitigation strategies
                var mitigationStrategies = await GetMitigationStrategiesForHotspots(hotspots);

                return new CalculationResult
                {
                    CalculationId = calculationId,
                    TrialId = request.TrialId,
                    TotalEmissions = totalEmissions,
                    Unit = "kg CO2e",
                    CalculationDate = calculation.CalculationDate,
                    Status = calculation.Status,
                    Breakdown = breakdown,
                    Hotspots = hotspots,
                    MitigationStrategies = mitigationStrategies
                };
            }
            catch (Exception ex)
            {
                calculation.Status = "Failed";
                calculation.ErrorMessage = ex.Message;
                await _context.SaveChangesAsync();
                throw;
            }
        }

        public async Task<IEnumerable<CalculationHistory>> GetCalculationHistoryAsync(string? trialId = null, string? userId = null)
        {
            var query = _context.CalculationHistories.AsQueryable();

            if (!string.IsNullOrEmpty(trialId))
                query = query.Where(c => c.TrialId == trialId);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(c => c.UserId == userId);

            return await query
                .OrderByDescending(c => c.CalculationDate)
                .ToListAsync();
        }

        public async Task<CalculationDetails?> GetCalculationDetailsAsync(Guid calculationId)
        {
            var calculation = await _context.CalculationHistories
                .Include(c => c.Activities)
                .Include(c => c.Hotspots)
                .ThenInclude(h => h.MitigationStrategy)
                .FirstOrDefaultAsync(c => c.Id == calculationId);

            if (calculation == null)
                return null;

            return new CalculationDetails
            {
                Calculation = calculation,
                Activities = calculation.Activities.ToList(),
                Hotspots = calculation.Hotspots.ToList()
            };
        }

        private List<HotspotInfo> IdentifyHotspots(List<ActivityBreakdown> breakdown)
        {
            var hotspots = new List<HotspotInfo>();

            foreach (var item in breakdown)
            {
                string severity;
                string? recommendation = null;

                if (item.Percentage >= 50)
                {
                    severity = "Critical";
                    recommendation = "Immediate action required. This activity represents over 50% of total emissions.";
                }
                else if (item.Percentage >= 25)
                {
                    severity = "High";
                    recommendation = "High priority for mitigation. Consider alternative approaches.";
                }
                else if (item.Percentage >= 10)
                {
                    severity = "Medium";
                    recommendation = "Moderate priority. Review for optimization opportunities.";
                }
                else
                {
                    severity = "Low";
                    recommendation = "Low priority. Monitor for future optimization.";
                }

                hotspots.Add(new HotspotInfo
                {
                    ActivityType = item.ActivityType,
                    Emissions = item.CalculatedEmissions,
                    Percentage = item.Percentage,
                    Severity = severity,
                    Recommendation = recommendation
                });
            }

            return hotspots.OrderByDescending(h => h.Percentage).ToList();
        }

        private async Task<List<MitigationStrategyInfo>> GetMitigationStrategiesForHotspots(List<HotspotInfo> hotspots)
        {
            var strategies = await _context.MitigationStrategies
                .Where(s => s.IsActive)
                .ToListAsync();

            var result = new List<MitigationStrategyInfo>();

            foreach (var hotspot in hotspots.Where(h => h.Severity == "High" || h.Severity == "Critical"))
            {
                var relevantStrategies = strategies.Where(s => 
                    s.ApplicableActivities != null && 
                    s.ApplicableActivities.Contains(hotspot.ActivityType))
                    .Take(2); // Limit to 2 strategies per hotspot

                foreach (var strategy in relevantStrategies)
                {
                    result.Add(new MitigationStrategyInfo
                    {
                        StrategyId = strategy.Id,
                        StrategyName = strategy.StrategyName,
                        Category = strategy.Category,
                        Description = strategy.Description,
                        PotentialReduction = strategy.PotentialReductionPercentage,
                        ImplementationSteps = strategy.ImplementationSteps
                    });
                }
            }

            return result.DistinctBy(s => s.StrategyId).ToList();
        }
    }
}
