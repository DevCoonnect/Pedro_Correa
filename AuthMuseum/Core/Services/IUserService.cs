using AuthMuseum.Core.Common;
using AuthMuseum.Domain.Entities;
using AuthMuseum.Domain.Requests;
using AuthMuseum.Domain.Responses;

namespace AuthMuseum.Core.Services;

public interface IUserService
{
    public Task<Result<User>> CreateUserAsync(CreateUserRequest request);
    public Task<Result<User?>> GetUserByIdAsync(long id);
    public Task<Result<User>> DeleteUserAsync(long id);
}