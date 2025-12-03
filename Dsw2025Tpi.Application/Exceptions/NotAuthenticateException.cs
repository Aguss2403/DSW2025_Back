namespace Dsw2025Tpi.Application.Exceptions;

public class NotAuthenticateException : ApplicationException
{
    public NotAuthenticateException() : base("Usuario no autenticado", 4010)
    {
    }
}