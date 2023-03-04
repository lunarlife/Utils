using System;

namespace Utils.Exceptions;

public class ConverterException : Exception
{
    public ConverterException(string msg) : base(msg)
    {

    }
}