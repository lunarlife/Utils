using System;

namespace Utils.Exceptions;

public class SerializeException : Exception
{
    public SerializeException(string msg) : base(msg)
    {

    }
}