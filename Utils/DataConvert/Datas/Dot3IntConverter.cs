using Utils.Dots;

namespace Utils.DataConvert.Datas;

public sealed class Dot3IntConverter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(Dot3Int).IsAssignableFrom(type);

    public byte[] Serialize(object o)
    {
        var dot = (Dot3Int)o;
        return DataConverter.Combine(DataConverter.Serialize(dot.X), DataConverter.Serialize(dot.Y),
            DataConverter.Serialize(dot.Z));
    }

    public object? Deserialize(Span<byte> data, Type type)
    {
        ushort index = 0;
        return new Dot3(DataConverter.Deserialize<int>(data, ref index),
            DataConverter.Deserialize<int>(data, ref index), DataConverter.Deserialize<int>(data, ref index));
    }

    public ushort Length => sizeof(int) * 3;
}