using Utils.Dots;

namespace Utils.DataConvert.Datas;

public sealed class Dot3Converter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(Dot3).IsAssignableFrom(type);

    public byte[] Serialize(object o)
    {
        var dot = (Dot3)o;
        return DataConverter.Combine(DataConverter.Serialize(dot.X), DataConverter.Serialize(dot.Y),
            DataConverter.Serialize(dot.Z));
    }

    public object? Deserialize(Span<byte> data, Type type)
    {
        ushort index = 0;
        return new Dot3(DataConverter.Deserialize<float>(data, ref index),
            DataConverter.Deserialize<float>(data, ref index), DataConverter.Deserialize<float>(data, ref index));
    }

    public ushort Length => sizeof(float) * 3;
}