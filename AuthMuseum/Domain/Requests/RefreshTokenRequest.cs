namespace AuthMuseum.Domain.Requests;

public record RefreshTokenRequest
{
    public required string Token { get; init; }
    public required string RefreshToken { get; init; }
}