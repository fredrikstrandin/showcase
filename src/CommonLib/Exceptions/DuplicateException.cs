using System;

namespace CommonLib.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string message) : base(message)
    {
    }
}
