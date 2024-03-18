using Domain.Models;

namespace Persistence;

public interface IProductRepository
{
    void Create(Product product);
    Product? Get(long id);
    IEnumerable<Product> GetAll();
    void Update(Product product);
    void Delete(long id);
}
