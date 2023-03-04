using System;
using Utils.Dots;

namespace Utils.DataConvert.Datas
{
    public sealed class Dot2IntConverter : IStaticDataConverter
    {
        public bool IsValidConvertor(Type type) => typeof(Dot2Int).IsAssignableFrom(type);
        public ushort Length => 8;

        public byte[] Serialize(object o)
        {
            var vec = (Dot2Int)o;
            return DataConverter.Combine(BitConverter.GetBytes(vec.X), BitConverter.GetBytes(vec.Y));
        }

        public object? Deserialize(Span<byte> data, Type currentType) => new Dot2Int(BitConverter.ToInt32(data), BitConverter.ToInt32(data[4..]));
    }
}