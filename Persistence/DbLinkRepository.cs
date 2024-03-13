using Domain.Models;

namespace Persistence;

public class DbLinkRepository : IRepository<Link>
{

    private ApplicationContext _context;

    public DbLinkRepository(ApplicationContext context)
    {
        _context = context;
    }

    public void Create(Link obj)
    {
        
    }

    public Link? Get(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Link> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Link obj)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}
