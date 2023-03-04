using System;

namespace Utils.DataConvert.Handlers;

public class DeserializeHandlerAttribute : HandlerAttribute
{
    public DeserializeHandlerAttribute(string methodName) : base(methodName)
    {
    }
}