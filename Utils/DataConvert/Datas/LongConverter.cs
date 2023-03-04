using System;

namespace Utils.DataConvert.Datas;

public class LongConverter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => type == typeof(long);

    public byte[] Serialize(object o) => BitConverter.GetBytes((long)o);

    public object? Deserialize(Span<byte> data, Type type) => BitConverter.ToInt64(data);

    public ushort Length => 8;
}