namespace Utils;

public class Monad<T>
{
    private readonly T _value;

    private Monad(T value)
    {
        _value = value;
    }

    public static Monad<T> From(T value) => new(value);
    public Monad<T1> FlatMap<T1>(Func<T, Monad<T1>> func) => func(_value);
    public Monad<T1> Map<T1>(Func<T, T1> func) => FlatMap(value => new Monad<T1>(func(value)));
    
}