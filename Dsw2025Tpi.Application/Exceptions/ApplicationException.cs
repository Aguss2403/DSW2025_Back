using System;

namespace Dsw2025Tpi.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    public int Code { get; }

    protected ApplicationException(string message, int code) : base(message)
    {
        Code = code;
    }
}