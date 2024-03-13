using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DbLinkRepository : ILinkRepository
{

    private ApplicationContext _context;

    public DbLinkRepository(ApplicationContext context)
    {
        _context = context;
    }

    public void Create(Link link)
    {
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
}
