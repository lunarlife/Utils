using System;

namespace Utils.Exceptions;

public class DeserializeException : Exception
{
    public DeserializeException(string msg) : base(msg)
    {

    }
}