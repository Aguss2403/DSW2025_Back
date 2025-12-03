namespace Dsw2025Tpi.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        // El código es opcional, por defecto es 4000
        public ValidationException(string message, int code = 4000) : base(message, code)
        {
        }
    }
}