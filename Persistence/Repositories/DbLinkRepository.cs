using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Exceptions;
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
        if (product == null)
            throw new EntityNotFoundException("Product not found");

        Product? upProduct = _productRepository.Get(link.UpProductId);
        if (upProduct == null)
            throw new EntityNotFoundException("UpProduct not found");

        if (FindCycle(product, upProduct))
            throw new ArgumentException("Products cannot form cycles");

        _context.Links.Add(link);
        _context.SaveChanges();
    }

    public Link? Get(long upProductId, long productId)
    {
        Link? link = _context.Links.Find(upProductId, productId);
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
        Link? oldLink = _context.Links.Find(link.UpProductId, link.ProductId);
        if (oldLink == null)
            throw new EntityNotFoundException("Link not found");
        _context.Entry(oldLink).CurrentValues.SetValues(link);
        _context.SaveChanges();
    }

    public void Delete(long upProductId, long productId)
    {
        Link? link = _context.Links.Find(upProductId, productId);
        if (link == null)
            throw new EntityNotFoundException("UpProduct not found");
        _context.Remove(link);
        _context.SaveChanges();
    }

    private bool FindCycle(Product product, Product upProduct)
    {
        if (product == upProduct)
            return true;

        foreach (Link productBelowLink in product.ProductsBelow)
        {
            Product? nextProduct = productBelowLink.Product
                ?? _productRepository.Get(productBelowLink.ProductId);
            if (nextProduct == null)
                throw new EntityNotFoundException("One of the products contains invalid link");
            if (FindCycle(nextProduct, upProduct))
                return true;
        }
        return false;
    }
}
