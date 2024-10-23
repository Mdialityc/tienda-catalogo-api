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
    public DbSet<ProductInCart> ProductInCars => Set<ProductInCart>();
    public DbSet<SessionToken> SessionTokens => Set<SessionToken>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductInCart>()
            .HasOne(pic => pic.Product)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductInCart>()
            .HasOne(pic => pic.SessionToken)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany()
            .UsingEntity(j => j.ToTable("OrderProducts")); 
    }
}