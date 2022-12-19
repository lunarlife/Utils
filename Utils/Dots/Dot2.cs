namespace Utils.Dots
{
    
public struct Dot2 : IEquatable<Dot2>
{
    public float X;
    public float Y;

    public static readonly Dot2 Zero = new(0, 0); 
    public static readonly Dot2 One = new(1, 1); 
    public static readonly Dot2 MinusOne = new(-1, -1); 
    public static readonly Dot2 Left = new(-1, 0); 
    public static readonly Dot2 Right = new(1, 0); 
    public static readonly Dot2 Up = new(0, 1); 
    public static readonly Dot2 Down = new(0, -1); 
    
    public Dot2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Dot2 Of(float value) => new(value, value);
    

    public Dot2 Normalized() => Normalize(this);
    public static Dot2 Normalize(Dot2 d)
    {
        var x = MathF.Pow(d.X, 2f);
        var y = MathF.Pow(d.Y, 2f);
        var num = x + y;
        if (d.X < 0) x *= -1;
        if (d.Y < 0) y *= -1;
        return new Dot2(x / num, y / num);
    }
    
    public static bool InDistance(Dot2 v1, Dot2 v2, float dist) => SqrMagnitude(v1,v2) < dist * dist;

    public static bool OutDistance(Dot2 v1, Dot2 v2, float dist) => SqrMagnitude(v1,v2) > dist * dist;
    public static float SqrMagnitude(Dot2 v1, Dot2 v2) => MathF.Pow(v1.X - v2.X, 2f) + MathF.Pow(v1.Y - v2.Y, 2f);

    public static Dot2 Lerp(Dot2 start, Dot2 end, float time) => LerpUnclamped(start, end, MathUtils.ClampOut(time, 0f, 1f));
    public static Dot2 LerpUnclamped(Dot2 start, Dot2 end, float time) => new(MathUtils.LerpUnclamped(start.X, end.X, time), MathUtils.LerpUnclamped(start.Y, end.Y, time));

    public static Dot2 operator +(Dot2 a, Dot2 b) => new(a.X + b.X, a.Y + b.Y);

    public static Dot2 operator -(Dot2 a, Dot2 b) => new(a.X - b.X, a.Y - b.Y);

    public static Dot2 operator *(Dot2 a, Dot2 b) => new(a.X * b.X, a.Y * b.Y);

    public static Dot2 operator /(Dot2 a, Dot2 b) => new(a.X / b.X, a.Y / b.Y);

    public static Dot2 operator -(Dot2 a) => new(-a.X, -a.Y);

    public static Dot2 operator *(Dot2 a, float d) => new(a.X * d, a.Y * d);

    public static Dot2 operator *(float d, Dot2 a) => new(a.X * d, a.Y * d);

    public static Dot2 operator /(Dot2 a, float d) => new(a.X / d, a.Y / d);

    public static bool operator ==(Dot2 lhs, Dot2 rhs)
    {
        var num1 = lhs.X - rhs.X; 
        var num2 = lhs.Y - rhs.Y;
        return num1 * (double) num1 + num2 * (double) num2 < 9.999999439624929E-11;
    }

    public static bool operator !=(Dot2 lhs, Dot2 rhs) => !(lhs == rhs);
    
    public static implicit operator Dot2(Dot3 vec) => new(vec.X, vec.Y);
    public static implicit operator Dot2(Dot2Int vec) => new(vec.X, vec.Y);
    public static implicit operator Dot2(Dot3Int vec) => new(vec.X, vec.Y);
    public static implicit operator Dot2((float x, float y) dot) => new(dot.x, dot.y);
    
    public static bool operator <(Dot2 a, Dot2 d) => a.X < d.X || a.Y < d.Y;

    public static bool operator >(Dot2 a, Dot2 d) => a.X > d.X || a.Y > d.Y;
    
    public static bool operator <=(Dot2 a, Dot2 d) => a.X <= d.X || a.Y <= d.Y;
    public static bool operator >=(Dot2 a, Dot2 d) => a.X >= d.X || a.Y >= d.Y;
    public override string ToString() => $"{{{X:F};{Y.ToString("F")}}}";

    public bool Equals(Dot2 other) => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object obj) => obj is Dot2 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);
}

}
