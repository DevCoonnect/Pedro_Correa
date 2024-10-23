using Microsoft.EntityFrameworkCore;

namespace AuthMuseum.Core.Repository.Impl;


public abstract class BaseRepository<T>(DbContext databaseContext) : IRepository<T> where T : class
{
    public void Add(T t)
    {
        databaseContext.Add(t);
    }

    public async Task AddAsync(T t)
    {
        await databaseContext.AddAsync(t);
    }

    public void Update(T t)
    {
        databaseContext.Update(t);
    }

    public void Delete(T t)
    {
        databaseContext.Remove(t);
    }

    public void SaveChanges()
    {
        databaseContext.SaveChanges();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await databaseContext.SaveChangesAsync() > 0;
    }
}
