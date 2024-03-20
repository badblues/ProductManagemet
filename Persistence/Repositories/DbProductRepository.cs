using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Exceptions;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories;

public class DbProductRepository : IProductRepository
{

    private readonly ApplicationContext _context;

    public DbProductRepository(ApplicationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Create(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public Product? Get(long id)
    {
        Product? product = _context.Products.Find(id);
        return product;
    }

    public IEnumerable<Product> GetAll()
    {
        List<Product> products = _context.Products
            .Include(p => p.ProductsBelow)
            .Include(p => p.UpProducts).ToList();
        return products;
    }

    public void Update(Product product)
    {
        Product? oldProduct = _context.Products.Find(product.Id);
        if (oldProduct == null)
            throw new EntityNotFoundException("Product not found");
        _context.Entry(oldProduct).CurrentValues.SetValues(product);
        _context.SaveChanges();
    }

    public void Delete(long id)
    {
        Product? product = _context.Products.Find(id);
        if (product == null)
            throw new EntityNotFoundException("Product not found");
        _context.Remove(product);
        _context.SaveChanges();
    }
}
