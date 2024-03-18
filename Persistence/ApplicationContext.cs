using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class ApplicationContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Link> Links { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductsBelow)
            .WithOne()
            .HasForeignKey(l => l.UpProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.UpProducts)
            .WithOne()
            .HasForeignKey(l => l.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Link>()
            .HasKey(link => new { link.UpProductId, link.ProductId });

        modelBuilder.Entity<Link>()
                .HasOne(l => l.UpProduct)
                .WithMany(p => p.ProductsBelow)
                .HasForeignKey(l => l.UpProductId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Link>()
                .HasOne(l => l.Product)
                .WithMany(p => p.UpProducts)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Product 1", Price = 800f },
            new Product { Id = 2, Name = "Product 2", Price = 100f },
            new Product { Id = 3, Name = "Product 3", Price = 400f },
            new Product { Id = 4, Name = "Product 4", Price = 400f },
            new Product { Id = 5, Name = "Product 5", Price = 300f },
            new Product { Id = 6, Name = "Product 6", Price = 20f },
            new Product { Id = 7, Name = "Product 7", Price = 1000f },
            new Product { Id = 8, Name = "Product 8", Price = 100f }
        );

        modelBuilder.Entity<Link>().HasData(
            new Link { UpProductId= 1, ProductId = 2, Count = 10 },
            new Link { UpProductId = 1, ProductId = 3, Count = 2 },
            new Link { UpProductId = 1, ProductId = 4, Count = 1 },
            new Link { UpProductId = 3, ProductId = 5, Count = 2 },
            new Link { UpProductId = 4, ProductId = 2, Count = 1 },
            new Link { UpProductId = 4, ProductId = 6, Count = 5 },
            new Link { UpProductId = 7, ProductId = 8, Count = 20 },
            new Link { UpProductId = 7, ProductId = 3, Count = 10 }
        );
    }
}
