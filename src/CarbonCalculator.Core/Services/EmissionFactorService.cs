using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Infrastructure.Data;

namespace CarbonCalculator.Core.Services
{
    public class EmissionFactorService : IEmissionFactorService
    {
        private readonly CarbonCalculatorContext _context;

        public EmissionFactorService(CarbonCalculatorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmissionFactor>> GetEmissionFactorsAsync(string? category = null, string? activityType = null)
        {
            var query = _context.EmissionFactors.Where(ef => ef.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(ef => ef.Category == category);

            if (!string.IsNullOrEmpty(activityType))
                query = query.Where(ef => ef.ActivityType.Contains(activityType));

            return await query
                .OrderBy(ef => ef.Category)
                .ThenBy(ef => ef.ActivityType)
                .ToListAsync();
        }

        public async Task<EmissionFactor?> GetEmissionFactorByIdAsync(int id)
        {
            return await _context.EmissionFactors
                .FirstOrDefaultAsync(ef => ef.Id == id && ef.IsActive);
        }

        public async Task<EmissionFactor> CreateEmissionFactorAsync(CreateEmissionFactorRequest request)
        {
            var emissionFactor = new EmissionFactor
            {
                Category = request.Category,
                SubCategory = request.SubCategory,
                ActivityType = request.ActivityType,
                EmissionFactorValue = request.EmissionFactorValue,
                Unit = request.Unit,
                Description = request.Description,
                Source = request.Source,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            _context.EmissionFactors.Add(emissionFactor);
            await _context.SaveChangesAsync();

            return emissionFactor;
        }

        public async Task<EmissionFactor?> GetEmissionFactorAsync(string activityType, string unit)
        {
            return await _context.EmissionFactors
                .FirstOrDefaultAsync(ef => 
                    ef.ActivityType == activityType && 
                    ef.Unit == unit && 
                    ef.IsActive);
        }
    }

    public class MitigationStrategyService : IMitigationStrategyService
    {
        private readonly CarbonCalculatorContext _context;

        public MitigationStrategyService(CarbonCalculatorContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MitigationStrategy>> GetMitigationStrategiesAsync(string? category = null, string? costCategory = null, string? difficulty = null)
        {
            var query = _context.MitigationStrategies.Where(ms => ms.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(ms => ms.Category == category);

            if (!string.IsNullOrEmpty(costCategory))
                query = query.Where(ms => ms.CostCategory == costCategory);

            if (!string.IsNullOrEmpty(difficulty))
                query = query.Where(ms => ms.ImplementationDifficulty == difficulty);

            return await query
                .OrderBy(ms => ms.Category)
                .ThenBy(ms => ms.StrategyName)
                .ToListAsync();
        }

        public async Task<MitigationStrategy?> GetMitigationStrategyByIdAsync(int id)
        {
            return await _context.MitigationStrategies
                .FirstOrDefaultAsync(ms => ms.Id == id && ms.IsActive);
        }
    }
}
