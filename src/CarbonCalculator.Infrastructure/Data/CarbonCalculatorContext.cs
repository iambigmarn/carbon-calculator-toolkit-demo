using Microsoft.EntityFrameworkCore;
using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.Infrastructure.Data
{
    public class CarbonCalculatorContext : DbContext
    {
        public CarbonCalculatorContext(DbContextOptions<CarbonCalculatorContext> options) : base(options)
        {
        }

        public DbSet<EmissionFactor> EmissionFactors { get; set; }
        public DbSet<CalculationHistory> CalculationHistories { get; set; }
        public DbSet<CalculationActivity> CalculationActivities { get; set; }
        public DbSet<MitigationStrategy> MitigationStrategies { get; set; }
        public DbSet<Hotspot> Hotspots { get; set; }
        public DbSet<TrialInformation> TrialInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure EmissionFactor
            modelBuilder.Entity<EmissionFactor>(entity =>
            {
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.ActivityType);
                entity.HasIndex(e => e.IsActive);
            });

            // Configure CalculationHistory
            modelBuilder.Entity<CalculationHistory>(entity =>
            {
                entity.HasIndex(e => e.TrialId);
                entity.HasIndex(e => e.CalculationDate);
                entity.HasIndex(e => e.UserId);
            });

            // Configure CalculationActivity
            modelBuilder.Entity<CalculationActivity>(entity =>
            {
                entity.HasIndex(e => e.CalculationId);
                entity.HasIndex(e => e.ActivityType);
                
                entity.HasOne(e => e.Calculation)
                      .WithMany(e => e.Activities)
                      .HasForeignKey(e => e.CalculationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure MitigationStrategy
            modelBuilder.Entity<MitigationStrategy>(entity =>
            {
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.IsActive);
            });

            // Configure Hotspot
            modelBuilder.Entity<Hotspot>(entity =>
            {
                entity.HasIndex(e => e.CalculationId);
                entity.HasIndex(e => e.Severity);
                
                entity.HasOne(e => e.Calculation)
                      .WithMany(e => e.Hotspots)
                      .HasForeignKey(e => e.CalculationId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.MitigationStrategy)
                      .WithMany(e => e.Hotspots)
                      .HasForeignKey(e => e.MitigationStrategyId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure TrialInformation
            modelBuilder.Entity<TrialInformation>(entity =>
            {
                entity.HasIndex(e => e.TrialId).IsUnique();
                entity.HasIndex(e => e.IsActive);
            });
        }
    }
}
