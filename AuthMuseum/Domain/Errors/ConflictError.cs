using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record ConflictError(string message) : Error(message)
{
}
