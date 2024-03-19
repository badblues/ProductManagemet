using Domain.Models;

namespace Persistence.Repositories.Interfaces;

public interface IProductRepository
{
    public void Create(Product product);

    public Product? Get(long id);

    public IEnumerable<Product> GetAll();

    public void Update(Product product);

    public void Delete(long id);
}
