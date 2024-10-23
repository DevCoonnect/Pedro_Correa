using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record UnauthorizedError(string message) : Error(message)
{
    
}