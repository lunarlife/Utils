using System;

namespace Utils.DataConvert
{
    public interface IConverter
    {
        public bool IsValidConvertor(Type type);

        public byte[] Serialize(object o);
        public object? Deserialize(Span<byte> data, Type type);

        public object? Deserialize(Span<byte> buffer, ushort index, ushort length, Type type)
        {
            if (index == 0 && length == buffer.Length) return Deserialize(buffer, type);
            return Deserialize(buffer.Slice(index, length), type);
        }
    }
}