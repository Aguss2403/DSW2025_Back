namespace Dsw2025Tpi.Application.Exceptions;

public class InvalidCredentialsException : ApplicationException
{
    public InvalidCredentialsException() : base("Credenciales inválidas.", 4001)
    {
    }
}