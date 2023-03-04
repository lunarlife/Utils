using System;

namespace Utils.DataConvert;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DataSwitchAttribute : Attribute
{
    public ushort Id { get; }

    public DataSwitchAttribute(ushort id)
    {
        Id = id;
    }
}