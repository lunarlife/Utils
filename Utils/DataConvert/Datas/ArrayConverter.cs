using System.Collections;
using Utils.Exceptions;

namespace Utils.DataConvert.Datas
{
    public sealed class ArrayConverter : IDynamicDataConverter
    {
        public bool IsValidConvertor(Type type) => typeof(Array).IsAssignableFrom(type);


        public byte[] Serialize(object o)
        {
            if (o is byte[] b) return b;
            var array = (Array)o;
            var type = o.GetType();
            if (DataConverter.GetConverterForType(type.GetElementType()!) is IStaticDataConverter c)
            {
                var arr = (stackalloc byte[c.Length * array.Length]);
                for (var i = 0; i < array.Length; i++)
                {
                    var serialize = c.Serialize(array.GetValue(i)).AsSpan();
                    for (var j = 0; j < serialize.Length; j++)
                        arr[i] = serialize[j];
                }
                return arr.ToArray();
            }
            
            var total = new List<byte>();
            for (var i = 0; i < array.Length; i++)
            {
                if (DataConverter.Serialize(array.GetValue(i)!) is not { } bytes) continue;
                total.AddRange(bytes);
            }
            if (total.Count == 0) return Array.Empty<byte>();
            return total.ToArray();
        }

        public object? Deserialize(Span<byte> data, Type type)
        {
            var arrType = type.GetElementType();
            if (arrType == null) throw new DeserializeException($"{type.Name} is not array");
            if (arrType == typeof(byte))
                return data.ToArray();

            var objects = new ArrayList();
            ushort deserialized = 0;
            while (deserialized < data.Length)
            {
                if (DataConverter.Deserialize(data, arrType, ref deserialized) is not { } deserialize) continue;
                objects.Add(deserialize);
            }
            var array = Array.CreateInstance(arrType, objects.Count);
            objects.CopyTo(array);
            return array;
        }
    }
}