using System.Reflection;
using Utils.Exceptions;
using Utils.Reflection;

namespace Utils;


public class TwoDimensionalArray
{
    private readonly Type _type;
    private Array _array;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public TwoDimensionalArray(int width, int height, Type type)
    {
        _type = type;
        Width = width;
        Height = height;
        _array = new object[width * height];
    }
    internal TwoDimensionalArray(int width, int height, Array array, Type type)
    {
        if (width * height != array.Length) throw new ArgumentOutOfRangeException();
        _type = type;
        Width = width;
        Height = height;
        _array = array;
    }
    internal static TwoDimensionalArray CreateArray(int width, int height, Array array, Type type)
    {
        var arr = (TwoDimensionalArray)typeof(TwoDimensionalArray<>).MakeGenericType(type)
            .GetConstructor(new [] {typeof(int), typeof(int), typeof(Array)})!.Invoke(new object[] { width, height, array });
        arr._array = array;
        arr.Width = width;
        arr.Height = width;
        return arr;
    }

    public object GetValue(int x, int y)
    {
        var index = y * Width + x;
        if (index < 0 || index > _array.Length) throw new IndexOutOfRangeException();
        return _array.GetValue(index);
    }
    public void SetValue(int x, int y, object o)
    {
        var index = y * Width + x;
        if (index < 0 || index > _array.Length) throw new IndexOutOfRangeException();
        if (!_type.IsInstanceOfType(o)) throw new ArrayException("invalid type");
        _array.SetValue(o, index);
    }

    public Array Copy() => _array.Copy();
    internal Array GetArray() => _array;
    public Type GetArrayType() => _type;
}
public sealed class TwoDimensionalArray<T> : TwoDimensionalArray
{
    public TwoDimensionalArray(int width, int height) : base(width, height, typeof(T))
    {
    }

    public TwoDimensionalArray(int width, int height, Array array) : base(width, height, array, typeof(T))
    {
        
    }

    public TwoDimensionalArray(T[,] array) : base(array.GetLength(0), array.GetLength(1), typeof(T))
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            SetValue(x, y, array[x, y]);
    }
    public T this[int x, int y]
    {
        get => (T)GetValue(x, y);
        set => SetValue(x, y, value);
    }
}