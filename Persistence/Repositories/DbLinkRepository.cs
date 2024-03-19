using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Interfaces;

namespace Persistence.Repositories;

public class DbLinkRepository : ILinkRepository
{

    private readonly ApplicationContext _context;
    private readonly IProductRepository _productRepository;

    public DbLinkRepository(
        ApplicationContext context,
        IProductRepository productRepository)
    {
        _context = context;
        _productRepository = productRepository;
    }

    public void Create(Link link)
    {
        Product? product = _productRepository.Get(link.ProductId);
        Product? upProduct = _productRepository.Get(link.UpProductId);

        if (FindCycle(product, upProduct))
            throw new ArgumentException("Products cannot form cycles");

        _context.Links.Add(link);
        _context.SaveChanges();
    }

    public Link? Get(long upProductId, long productId)
    {
        Link? link = _context.Links
            .SingleOrDefault(l => l.UpProductId == upProductId && l.ProductId == productId);
        return link;
    }

    public IEnumerable<Link> GetAll()
    {
        List<Link> links = _context.Links
            .Include(l => l.Product)
            .Include(l => l.UpProduct)
            .ToList();
        return links;
    }

    public void Update(Link link)
    {
        Link? oldLink = _context.Links
            .SingleOrDefault(l => l.UpProductId == link.UpProductId && l.ProductId == link.ProductId);
        if (oldLink != null)
        {
            _context.Entry(oldLink).CurrentValues.SetValues(link);
            _context.SaveChanges();
        }
    }

    public void Delete(long upProductId, long productId)
    {
        Link? link = _context.Links
            .SingleOrDefault(l => l.UpProductId == upProductId && l.ProductId == productId);
        if (link != null)
        {
            _context.Remove(link);
            _context.SaveChanges();
        }
    }

    private bool FindCycle(Product product, Product upProduct)
    {
        if (product == upProduct)
            return true;

        foreach (Link productBelow in product.ProductsBelow)
        {
            if (FindCycle(productBelow.Product, upProduct))
                return true;
        }
        return false;
    }
}
