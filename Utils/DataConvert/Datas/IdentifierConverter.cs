using System;

namespace Utils.DataConvert.Datas
{
    public sealed class IdentifierConverter : IStaticDataConverter
    {
        public bool IsValidConvertor(Type type) => typeof(Identifier).IsAssignableFrom(type);
        public ushort Length => 16;
        public byte[] Serialize(object o) => ((Identifier)o).ToByteArray();
        public object? Deserialize(Span<byte> data, Type currentType) => new Identifier(new Guid(data));
    }
}