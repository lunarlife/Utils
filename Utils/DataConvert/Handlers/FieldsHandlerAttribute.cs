using System;

namespace Utils.DataConvert.Handlers;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public abstract class HandlerAttribute : Attribute
{
    public string MethodName { get; }

    public HandlerAttribute(string methodName)
    {
        MethodName = methodName;
    }
}