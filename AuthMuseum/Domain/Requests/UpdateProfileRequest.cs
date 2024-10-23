namespace AuthMuseum.Domain.Requests;

public record UpdateProfileRequest
{
    public string? Name { get; init; }
    
    public List<string>? Permissions { get; init; }
}