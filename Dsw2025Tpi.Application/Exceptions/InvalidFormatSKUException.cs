namespace Dsw2025Tpi.Application.Exceptions;

public class InvalidFormatSKUException : ApplicationException
{
    public InvalidFormatSKUException()
        : base("El formato del SKU es inválido. Debe ser 'SKU-XXXX'.", 3000)
    {
    }
}