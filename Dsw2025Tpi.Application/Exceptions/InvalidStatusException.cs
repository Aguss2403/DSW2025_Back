namespace Dsw2025Tpi.Application.Exceptions;

public class InvalidStatusException : ApplicationException
{
    public InvalidStatusException(string message) : base(message, 4001)
    {
    }
}