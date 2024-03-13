using Domain.Models;

namespace Persistence;

public interface ILinkRepository
{
    public void Create(Link link);

    public Link? Get(long upProductId, long productId);

    public IEnumerable<Link> GetAll();

    public void Update(Link link);

    public void Delete(long upProductId, long productId);
}
