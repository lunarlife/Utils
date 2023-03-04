using System;

namespace Utils.DataConvert.DataUse;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class DataConvertUseAttribute : Attribute
{
    public DataType Types { get; }

    public DataConvertUseAttribute(DataType types)
    {
        Types = types;
    }
}