using System.Threading.Tasks;
using Domain.Models;

namespace Persistence;

public class DbProductRepository : IRepository<Product>
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
        List<Product> products = _context.Products.ToList();
        return products;
    }

    public void Update(Product product)
    {
        var res = _context.Products.SingleOrDefault(p => p.Id == product.Id);
        if (res != null)
        {
            _context.Entry(res).CurrentValues.SetValues(product);
            _context.SaveChanges();
        }
    }

    public void Delete(Guid id)
    {
        Product? product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Remove(product);
            _context.SaveChanges();
        }
    }
}
