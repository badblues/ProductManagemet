namespace Persistence;

public interface IRepository<T>
{
    void Create(T obj);
    T? Get(long id);
    IEnumerable<T> GetAll();
    void Update(T obj);
    void Delete(Guid id);
}
