using System.ComponentModel.DataAnnotations;

namespace AuthMuseum.Domain.Requests;

public record AuthRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    
    public required string Password { get; init; }
}