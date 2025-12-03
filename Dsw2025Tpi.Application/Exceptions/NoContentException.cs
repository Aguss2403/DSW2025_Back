namespace Dsw2025Tpi.Application.Exceptions;

public class NoContentException : ApplicationException
{
    public NoContentException(string message = "No hay contenido para mostrar.")
        : base(message, 2004)
    {
    }
}