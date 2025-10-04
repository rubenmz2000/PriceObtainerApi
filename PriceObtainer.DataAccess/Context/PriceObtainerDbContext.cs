using Microsoft.EntityFrameworkCore;
using PriceObtainer.Library.Models;

namespace PriceObtainer.DataAccess.Context;

public class PriceObtainerDbContext : DbContext
{
    public PriceObtainerDbContext(DbContextOptions<PriceObtainerDbContext> options) : base(options) {}
    
    public DbSet<PriceSample> Prices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PriceSample>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Symbol)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Timestamp)
                .IsRequired();

            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,8)");

            entity.HasIndex(e => new { e.Symbol, e.Timestamp });
        });
    }
}