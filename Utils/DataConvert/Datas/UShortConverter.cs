using System;
using Utils.DataConvert;

namespace Utils.DataConvert.Datas;

public sealed class UShortConverter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(ushort).IsAssignableFrom(type);
    public ushort Length => 2;

    public byte[] Serialize(object o) => BitConverter.GetBytes((ushort)o);

    public object? Deserialize(Span<byte> data, Type type) => BitConverter.ToUInt16(data);
}