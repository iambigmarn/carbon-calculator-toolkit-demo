using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Infrastructure.Data;

namespace CarbonCalculator.Core.Services;

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

        return await query.OrderBy(ef => ef.Category).ThenBy(ef => ef.ActivityType).ToListAsync();
    }

    public async Task<EmissionFactor?> GetEmissionFactorByIdAsync(int id)
    {
        return await _context.EmissionFactors
            .FirstOrDefaultAsync(ef => ef.Id == id && ef.IsActive);
    }

    public async Task<EmissionFactor?> GetEmissionFactorByActivityTypeAndUnitAsync(string activityType, string unit)
    {
        return await _context.EmissionFactors
            .FirstOrDefaultAsync(ef => ef.ActivityType == activityType && ef.Unit == unit && ef.IsActive);
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
            LastUpdated = DateTime.UtcNow,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.EmissionFactors.Add(emissionFactor);
        await _context.SaveChangesAsync();

        return emissionFactor;
    }
}
