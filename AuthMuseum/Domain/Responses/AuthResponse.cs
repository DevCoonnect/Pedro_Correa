namespace AuthMuseum.Domain.Responses;

public record AuthResponse
{
    public bool Authenticated { get; init; }
    public string? CreationDate { get; init; }
    public string? ExpirationDate { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}