using AuthMuseum.Core.Common;
using AuthMuseum.Core.Helpers;
using AuthMuseum.Core.Repository;
using AuthMuseum.Domain.Entities;
using AuthMuseum.Domain.Errors;
using AuthMuseum.Domain.Requests;

namespace AuthMuseum.Core.Services.Impl;

public class UserService(IUserRepository userRepository, IPermissionRepository permissionRepository) : IUserService
{
    
    public async Task<Result<User>> CreateUserAsync(CreateUserRequest request)
    {
        var checkEmail = await userRepository.GetByEmailAsync(request.Email);
        if (checkEmail != null)
        {
            return new ConflictError($"Already exists a user with the email '{request.Email}'!");
        }

        var password = request.Password.ToSHA256();

        var permissions = new List<Permission>();
        if (request.Permissions != null)
        {  
            var result = await permissionRepository.TryGetPermissionsByIdAsync(request.Permissions.Select(x => x.ToString()));

            if (!result.Success)
            {
                return new UnprocessableEntityError(
                    $"Permissions '{string.Join(", ", result.InvalidPermissions)}' are not recognized!");
            }

            permissions = result.ParsedPermissions;
        }

        var user = new User()
        {
            Email = request.Email,
            IndividualPermissions = permissions,
            Name = request.Name,
            Password = password,
            Profile = request.Profile
        };
        
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        
        return user;
    }

    public async Task<Result<User?>> GetUserByIdAsync(long id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new NotFoundError($"User with Id {id} not found!");
        }

        return user;
    }

    public async Task<Result<User>> DeleteUserAsync(long id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new NotFoundError($"User with Id {id} not found!");
        }
        
        userRepository.Delete(user);
        await userRepository.SaveChangesAsync();

        return user;
    }
}