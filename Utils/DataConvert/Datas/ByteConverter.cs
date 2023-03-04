namespace Utils.DataConvert.Datas;

public sealed class ByteConverter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(byte).IsAssignableFrom(type);

    public byte[] Serialize(object o) => new [] { (byte)o };

    public object? Deserialize(Span<byte> data, Type type) => data[0];

    public ushort Length => sizeof(byte);
}