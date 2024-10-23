namespace AuthMuseum.Core.Common;
public record Error(string message, Exception? exception = null)
{
    public string Message { get; } = message;

    public Exception? Exception { get; } = exception;
}