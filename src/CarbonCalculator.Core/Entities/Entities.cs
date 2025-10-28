using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarbonCalculator.Core.Entities
{
    public class EmissionFactor
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string SubCategory { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string ActivityType { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(10,6)")]
        public decimal EmissionFactorValue { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [MaxLength(200)]
        public string? Source { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CalculationHistory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(100)]
        public string TrialId { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? UserId { get; set; }
        
        [MaxLength(200)]
        public string? CalculationName { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(12,4)")]
        public decimal TotalEmissions { get; set; }
        
        [MaxLength(50)]
        public string Unit { get; set; } = "kg CO2e";
        
        public DateTime CalculationDate { get; set; } = DateTime.UtcNow;
        
        [MaxLength(50)]
        public string Status { get; set; } = "Completed";
        
        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<CalculationActivity> Activities { get; set; } = new List<CalculationActivity>();
        public virtual ICollection<Hotspot> Hotspots { get; set; } = new List<Hotspot>();
    }

    public class CalculationActivity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public Guid CalculationId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string ActivityType { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(12,4)")]
        public decimal Quantity { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Unit { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(10,6)")]
        public decimal EmissionFactor { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(12,4)")]
        public decimal CalculatedEmissions { get; set; }
        
        [MaxLength(500)]
        public string? ActivityDescription { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        [ForeignKey("CalculationId")]
        public virtual CalculationHistory Calculation { get; set; } = null!;
    }

    public class MitigationStrategy
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string StrategyName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string? ImplementationSteps { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? PotentialReductionPercentage { get; set; }
        
        [MaxLength(50)]
        public string? CostCategory { get; set; }
        
        [MaxLength(50)]
        public string? ImplementationDifficulty { get; set; }
        
        [MaxLength(1000)]
        public string? ApplicableActivities { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public virtual ICollection<Hotspot> Hotspots { get; set; } = new List<Hotspot>();
    }

    public class Hotspot
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public Guid CalculationId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string ActivityType { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(12,4)")]
        public decimal Emissions { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentageOfTotal { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Severity { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Recommendation { get; set; }
        
        public int? MitigationStrategyId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("CalculationId")]
        public virtual CalculationHistory Calculation { get; set; } = null!;
        
        [ForeignKey("MitigationStrategyId")]
        public virtual MitigationStrategy? MitigationStrategy { get; set; }
    }

    public class TrialInformation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TrialId { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(300)]
        public string TrialName { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? PrincipalInvestigator { get; set; }
        
        [MaxLength(300)]
        public string? Institution { get; set; }
        
        [MaxLength(50)]
        public string? TrialPhase { get; set; }
        
        public int? PatientCount { get; set; }
        
        public int? DurationMonths { get; set; }
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
