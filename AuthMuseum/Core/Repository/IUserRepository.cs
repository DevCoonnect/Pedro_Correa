using AuthMuseum.Domain.Entities;

namespace AuthMuseum.Core.Repository;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);

    public Task<User?> GetByIdAsync(long id);
}