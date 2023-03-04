using System.Collections;
using System.Globalization;
using System.Reflection;
using Utils.Exceptions;

namespace Utils.DataConvert.Datas;

public class TwoDimensionalArrayConverter : IDynamicDataConverter
{
    public bool IsValidConvertor(Type type) => typeof(TwoDimensionalArray).IsAssignableFrom(type);

    public byte[] Serialize(object o)
    {
        if (o is byte[] b) return b;
        var array = (TwoDimensionalArray)o;
        var type = o.GetType();
        byte[] serializedArray;
        if (DataConverter.GetConverterForType(type.GetGenericArguments()[0]) is IStaticDataConverter c)
        {
            serializedArray = new byte[c.Length * array.Width * array.Height + 4];
            BitConverter.GetBytes((ushort)array.Width).CopyTo(serializedArray, 0);
            BitConverter.GetBytes((ushort)array.Height).CopyTo(serializedArray, 2);
            for (var x = 0; x < array.Width; x++)
            {
                for (var y = 0; y < array.Height; y++)
                { 
                    var serialize = c.Serialize(array.GetValue(x, y));
                    serialize.CopyTo(serializedArray, 4 + y * array.Width + x);
                    /*for (var j = 0; j < serialize.Length; j++)
                        arr[x] = serialize[j];*/
                }
            }
        }
        else
        {
            var list = new List<byte>();
            list.AddRange(BitConverter.GetBytes((ushort)array.Width));
            list.AddRange(BitConverter.GetBytes((ushort)array.Height));
            for (var x = 0; x < array.Width; x++)
            {
                for (var y = 0; y < array.Height; y++)
                {
                    var serialize = DataConverter.Serialize(array.GetValue(x, y));
                    list.AddRange(serialize);
                }
            }
            serializedArray = list.ToArray();
        }
        return serializedArray;
    }

    public object? Deserialize(Span<byte> data, Type type)
    {
        var arrType = type.GetGenericArguments()[0];
        if (arrType == null) throw new DeserializeException($"{type.Name} is not array");
        ushort deserialized = 0;
        var width = DataConverter.Deserialize<ushort>(data, ref deserialized);
        var height = DataConverter.Deserialize<ushort>(data, ref deserialized);
        if (arrType == typeof(byte))
            return TwoDimensionalArray.CreateArray(width, height, data.ToArray(), typeof(byte));//not working
        var array = (TwoDimensionalArray)type.GetConstructor(new[] { typeof(int), typeof(int) })!.Invoke(new object[] { width, height});
        for (var x = 0; x < width; x++)
        {
            if(deserialized < data.Length) break;
            for (var y = 0; y < height; y++)
            {
                if(deserialized < data.Length) break;
                array.SetValue(x, y, DataConverter.Deserialize(data, arrType, ref deserialized)!);
            }
        }

        return array;
    }
}