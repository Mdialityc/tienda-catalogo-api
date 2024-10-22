using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data.Models;

namespace tienda_catalogo_api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductInCar> ProductInCars => Set<ProductInCar>();
    public DbSet<SessionToken> SessionTokens => Set<SessionToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}