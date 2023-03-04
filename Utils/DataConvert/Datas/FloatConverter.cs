using System;
using Utils.DataConvert;

namespace Utils.DataConvert.Datas
{
    public sealed class FloatConverter : IStaticDataConverter
    {
        public bool IsValidConvertor(Type type) => typeof(float).IsAssignableFrom(type);
        public ushort Length => 4;
        public byte[] Serialize(object o) => BitConverter.GetBytes((float)o);
        public object? Deserialize(Span<byte> data, Type currentType) => BitConverter.ToSingle(data);
    }
}