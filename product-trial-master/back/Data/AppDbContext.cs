using System;
using Microsoft.EntityFrameworkCore;
using ProductModel.Model;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
	public DbSet<Product> Products { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        //precision du prix
		modelBuilder.Entity<Product>()
		.Property(p => p.Price)
		.HasPrecision(18,2);

        //Conversion enum en string dans la DB
        modelBuilder.Entity<Product>()
        .Property(p => p.InventoryStatus)
        .HasConversion(
            v => v.ToString(),
            v => (InventoryStatus)Enum.Parse(typeof(InventoryStatus), v));

        //Precision temps CreatedAT
        modelBuilder.Entity<Product>()
        .Property(p => p.CreatedAt)
        .HasColumnType("datetime2(0)");

        //Precision temps UpdatedAT
        modelBuilder.Entity<Product>()
        .Property(p => p.UpdatedAt)
        .HasColumnType("datetime2(0)");

        base.OnModelCreating(modelBuilder);
	}
}
