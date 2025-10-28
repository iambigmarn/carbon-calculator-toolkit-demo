using System.ComponentModel.DataAnnotations;

namespace CarbonCalculator.Core.Entities;

public class EmissionFactor
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string SubCategory { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string ActivityType { get; set; } = string.Empty;
    
    [Required]
    public decimal EmissionFactorValue { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Unit { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(200)]
    public string? Source { get; set; }
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class MitigationStrategy
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string StrategyName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [StringLength(2000)]
    public string? ImplementationSteps { get; set; }
    
    public decimal PotentialReductionPercentage { get; set; }
    
    [Required]
    [StringLength(50)]
    public string CostCategory { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string ImplementationDifficulty { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? ApplicableActivities { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Calculation
{
    public string CalculationId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string TrialId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string UserId { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? CalculationName { get; set; }
    
    public decimal TotalEmissions { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Unit { get; set; } = "kg CO2e";
    
    public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Completed";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<CalculationActivity> Activities { get; set; } = new List<CalculationActivity>();
    public virtual ICollection<CalculationHotspot> Hotspots { get; set; } = new List<CalculationHotspot>();
    public virtual ICollection<CalculationMitigationStrategy> MitigationStrategies { get; set; } = new List<CalculationMitigationStrategy>();
}

public class CalculationActivity
{
    public int Id { get; set; }
    
    [Required]
    public string CalculationId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string ActivityType { get; set; } = string.Empty;
    
    public decimal Quantity { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Unit { get; set; } = string.Empty;
    
    public decimal EmissionFactor { get; set; }
    public decimal CalculatedEmissions { get; set; }
    public decimal Percentage { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    // Navigation property
    public virtual Calculation Calculation { get; set; } = null!;
}

public class CalculationHotspot
{
    public int Id { get; set; }
    
    [Required]
    public string CalculationId { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string ActivityType { get; set; } = string.Empty;
    
    public decimal Emissions { get; set; }
    public decimal Percentage { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Severity { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Recommendation { get; set; }
    
    // Navigation property
    public virtual Calculation Calculation { get; set; } = null!;
}

public class CalculationMitigationStrategy
{
    public int Id { get; set; }
    
    [Required]
    public string CalculationId { get; set; } = string.Empty;
    
    public int StrategyId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string StrategyName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public decimal? PotentialReduction { get; set; }
    
    [StringLength(2000)]
    public string? ImplementationSteps { get; set; }
    
    // Navigation properties
    public virtual Calculation Calculation { get; set; } = null!;
    public virtual MitigationStrategy Strategy { get; set; } = null!;
}
