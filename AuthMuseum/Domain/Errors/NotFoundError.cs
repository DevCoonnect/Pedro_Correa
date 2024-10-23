using AuthMuseum.Core.Common;

namespace AuthMuseum.Domain.Errors;

public record NotFoundError(string message) : Error(message)
{
}
