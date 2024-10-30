using System.ComponentModel.DataAnnotations;
using AuthMuseum.Domain.Enums;

namespace AuthMuseum.Domain.Requests;

public record CreateUserRequest
{
    [StringLength(32, MinimumLength = 3, ErrorMessage = "{0} must be between {0} and {2} characters")]
    public required string Name { get; init; }
    
    [EmailAddress]
    public required string Email { get; init; }
    
    [StringLength(255, MinimumLength = 3, ErrorMessage = "{0} must be between {0} and {2} characters")]
    public required string Password { get; init; }

    public Profiles Profile { get; init; } = Profiles.NONE;
    
    public List<Permissions>? Permissions { get; init; }
}