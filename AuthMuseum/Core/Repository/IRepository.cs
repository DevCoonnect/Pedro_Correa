namespace AuthMuseum.Core.Repository;

public interface IRepository<in T>
{
    void Add(T t);
    Task AddAsync(T t);
    void Delete(T t);
    Task<bool> SaveChangesAsync();
}