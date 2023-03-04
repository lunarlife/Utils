namespace Utils.DataConvert.Datas;

public sealed class MiniFloatConverter : IStaticDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(MiniFloat).IsAssignableFrom(type);

    public byte[] Serialize(object o) => BitConverter.GetBytes(((MiniFloat)o).GetValue());

    public object? Deserialize(Span<byte> data, Type type) => MiniFloat.FromValue(BitConverter.ToInt16(data));

    public ushort Length => sizeof(short);
}