namespace Utils;

public readonly struct MiniFloat
{
    public bool Equals(MiniFloat other) => _value == other._value;

    public override bool Equals(object? obj) => obj is MiniFloat other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public const float MaxValue = short.MaxValue / 100f;
    public const float MinValue = short.MinValue / 100f;
    private readonly short _value;

    public static implicit operator MiniFloat(float value)
    {
        Validate(value);
        return new MiniFloat((short)(value * 100f));
    }
    public static implicit operator float(MiniFloat value) => value._value / 100f;
    public static implicit operator double(MiniFloat value) => value._value / 100d;
    public static implicit operator int(MiniFloat value) => (int)(value._value / 100f);
    public static implicit operator short(MiniFloat value) => (short)(value._value / 100f);

    public static MiniFloat operator -(MiniFloat left, MiniFloat right) => left._value - right._value;
    public static MiniFloat operator +(MiniFloat left, MiniFloat right) => left._value + right._value;
    
    public static MiniFloat operator *(MiniFloat left, MiniFloat right) => (float)left * (float)right;
    public static MiniFloat operator /(MiniFloat left, MiniFloat right) => (float)left / (float)right;

    public static bool operator ==(MiniFloat left, MiniFloat right) => left._value == right._value;

    public static bool operator !=(MiniFloat left, MiniFloat right) => !(left == right);

    private MiniFloat(short value)
    {
        _value = value;
    }

    public short GetValue() => _value;
    public static MiniFloat FromValue(short value) => new(value);

    private static void Validate(float value)
    {
        if (value * 100f is > MaxValue or < MinValue) throw new ArgumentOutOfRangeException();
    }
}