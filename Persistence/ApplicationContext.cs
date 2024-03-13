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

        modelBuilder.Entity<Link>().HasKey(link => new { link.UpProductId, link.ProductId });

        modelBuilder.Entity<Link>()
                .HasOne(l => l.UpProduct)
                .WithMany(p => p.ProductsBelow)
                .HasForeignKey(l => l.UpProductId)
                .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Link>()
                    .HasOne(l => l.Product)
                    .WithMany()
                    .HasForeignKey(l => l.ProductId)
                    .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Product 1", Price = 10.0f },
            new Product { Id = 2, Name = "Product 2", Price = 20.0f }
        );

        modelBuilder.Entity<Link>().HasData(
            new Link { UpProductId= 1, ProductId = 2, Count = 1 }
        );
    }
}
