using System;
using Utils.DataConvert;

namespace Utils.DataConvert.Datas;

public class UintConverter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(uint) == type;
    public byte[] Serialize(object o) => BitConverter.GetBytes((uint)o);
    public object? Deserialize(Span<byte> data, Type type) => BitConverter.ToUInt32(data);
    public ushort Length => 4;
}