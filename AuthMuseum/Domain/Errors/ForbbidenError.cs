using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record ForbiddenError(string message) : Error(message)
{
}
