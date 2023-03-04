using System;
using System.Text;
using Version = Utils.Version;

namespace Utils.DataConvert.Datas
{
    public sealed class VersionConverter : IDynamicDataConverter
    {
        public bool IsValidConvertor(Type type) => typeof(Version).IsAssignableFrom(type);
        public byte[] Serialize(object o) => Encoding.UTF8.GetBytes(((Version)o).ToString());
        public object? Deserialize(Span<byte> bytes, Type type) => new Version(Encoding.UTF8.GetString(bytes));
    }
}