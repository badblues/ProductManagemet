using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DbProductRepository : IProductRepository
{

    private ApplicationContext _context;

    public DbProductRepository(ApplicationContext context)
    {
        _context = context;
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
        List<Product> products = _context.Products.Include(p => p.ProductsBelow).ToList();
        return products;
    }

    public void Update(Product product)
    {
        Product? oldProduct = _context.Products.SingleOrDefault(p => p.Id == product.Id);
        if (oldProduct != null)
        {
            _context.Entry(oldProduct).CurrentValues.SetValues(product);
            _context.SaveChanges();
        }
    }

    public void Delete(long id)
    {
        Product? product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Remove(product);
            _context.SaveChanges();
        }
    }
}
