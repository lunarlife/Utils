using System;

namespace Utils.DataConvert.Handlers;

public class SerializeHandlerAttribute : HandlerAttribute
{
    public SerializeHandlerAttribute(string methodName) : base(methodName)
    {
    }
}