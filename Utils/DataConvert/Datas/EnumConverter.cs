using System;
using System.Linq;
using Utils.DataConvert;

namespace Utils.DataConvert.Datas
{
    public sealed class EnumConverter : IStaticDataConverter
    {
        public bool IsValidConvertor(Type type) => type.GetCustomAttributes(false).FirstOrDefault(a => a is FlagsAttribute) == null && typeof(Enum).IsAssignableFrom(type);

        public ushort Length => sizeof(ushort);
        public byte[] Serialize(object o) => BitConverter.GetBytes((ushort)Array.IndexOf(Enum.GetValues(o.GetType()), o));
        public object? Deserialize(Span<byte> data, Type type) => Enum.GetValues(type).GetValue(BitConverter.ToUInt16(data));
    }
}