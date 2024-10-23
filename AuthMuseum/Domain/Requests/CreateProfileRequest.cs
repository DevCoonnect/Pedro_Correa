namespace AuthMuseum.Domain.Requests;

public record CreateProfileRequest
{
    public required string Name { get; init; }
    public List<string> Permissions { get; init; } = new ();
}