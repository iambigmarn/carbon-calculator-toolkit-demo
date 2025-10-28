using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Infrastructure.Data;

namespace CarbonCalculator.Core.Services;

public class MitigationStrategyService : IMitigationStrategyService
{
    private readonly CarbonCalculatorContext _context;

    public MitigationStrategyService(CarbonCalculatorContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MitigationStrategy>> GetMitigationStrategiesAsync(string? category = null, string? costCategory = null, string? difficulty = null)
    {
        var query = _context.MitigationStrategies.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(ms => ms.Category == category);

        if (!string.IsNullOrEmpty(costCategory))
            query = query.Where(ms => ms.CostCategory == costCategory);

        if (!string.IsNullOrEmpty(difficulty))
            query = query.Where(ms => ms.ImplementationDifficulty == difficulty);

        return await query.OrderBy(ms => ms.Category).ThenBy(ms => ms.StrategyName).ToListAsync();
    }

    public async Task<IEnumerable<MitigationStrategy>> GetMitigationStrategiesByActivityTypeAsync(string activityType)
    {
        return await _context.MitigationStrategies
            .Where(ms => ms.ApplicableActivities != null && ms.ApplicableActivities.Contains(activityType))
            .OrderByDescending(ms => ms.PotentialReductionPercentage)
            .ToListAsync();
    }

    public async Task<MitigationStrategy?> GetMitigationStrategyByIdAsync(int id)
    {
        return await _context.MitigationStrategies
            .FirstOrDefaultAsync(ms => ms.Id == id);
    }
}
