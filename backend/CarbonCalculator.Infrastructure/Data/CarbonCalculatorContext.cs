using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.Infrastructure.Data;

public class CarbonCalculatorContext : DbContext
{
    public CarbonCalculatorContext(DbContextOptions<CarbonCalculatorContext> options) : base(options)
    {
    }

    public DbSet<EmissionFactor> EmissionFactors { get; set; }
    public DbSet<MitigationStrategy> MitigationStrategies { get; set; }
    public DbSet<Calculation> Calculations { get; set; }
    public DbSet<CalculationActivity> CalculationActivities { get; set; }
    public DbSet<CalculationHotspot> CalculationHotspots { get; set; }
    public DbSet<CalculationMitigationStrategy> CalculationMitigationStrategies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Calculation entity
        modelBuilder.Entity<Calculation>(entity =>
        {
            entity.HasKey(e => e.CalculationId);
            entity.Property(e => e.CalculationId).HasMaxLength(100);
            entity.Property(e => e.TrialId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.UserId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CalculationName).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            
            entity.HasIndex(e => e.TrialId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CalculationDate);
        });

        // Configure CalculationActivity entity
        modelBuilder.Entity<CalculationActivity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CalculationId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ActivityType).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            
            entity.HasOne(e => e.Calculation)
                  .WithMany(c => c.Activities)
                  .HasForeignKey(e => e.CalculationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CalculationHotspot entity
        modelBuilder.Entity<CalculationHotspot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CalculationId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ActivityType).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Severity).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Recommendation).HasMaxLength(1000);
            
            entity.HasOne(e => e.Calculation)
                  .WithMany(c => c.Hotspots)
                  .HasForeignKey(e => e.CalculationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure CalculationMitigationStrategy entity
        modelBuilder.Entity<CalculationMitigationStrategy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CalculationId).HasMaxLength(100).IsRequired();
            entity.Property(e => e.StrategyName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImplementationSteps).HasMaxLength(2000);
            
            entity.HasOne(e => e.Calculation)
                  .WithMany(c => c.MitigationStrategies)
                  .HasForeignKey(e => e.CalculationId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Strategy)
                  .WithMany()
                  .HasForeignKey(e => e.StrategyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure EmissionFactor entity
        modelBuilder.Entity<EmissionFactor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Category).HasMaxLength(100).IsRequired();
            entity.Property(e => e.SubCategory).HasMaxLength(100).IsRequired();
            entity.Property(e => e.ActivityType).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Source).HasMaxLength(200);
            
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.ActivityType);
        });

        // Configure MitigationStrategy entity
        modelBuilder.Entity<MitigationStrategy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StrategyName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImplementationSteps).HasMaxLength(2000);
            entity.Property(e => e.CostCategory).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ImplementationDifficulty).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ApplicableActivities).HasMaxLength(1000);
            
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.CostCategory);
        });
    }
}
