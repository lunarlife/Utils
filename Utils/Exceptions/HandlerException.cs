using System;

namespace Utils.Exceptions;

public class HandlerException : Exception
{
    public HandlerException(string msg) : base(msg)
    {

    }
}