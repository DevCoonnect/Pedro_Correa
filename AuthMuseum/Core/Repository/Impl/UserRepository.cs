using AuthMuseum.Domain.Entities;
using AuthMuseum.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthMuseum.Core.Repository.Impl;

public class UserRepository(PostgresDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly DbSet<User> _users = context.Set<User>();


    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _users.FirstOrDefaultAsync(u => string.Equals(email.ToLower(), u.Email.ToLower()));

        return user;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        var user = await _users.FindAsync(id);

        return user;
    }
}