namespace Utils.Dots;

public struct Dot3 : IEquatable<Dot3>
{

    public float X;
    public float Y;
    public float Z;
    
    public Dot3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public Dot3(float x, float y)
    {
        X = x;
        Y = y;
        Z = 0.0f;
    }
    public Dot3 Normalized() => Normalize(this);
    public static Dot3 Normalize(Dot3 d)
    {
        var x = MathF.Pow(d.X, 2f);
        var y = MathF.Pow(d.Y, 2f);
        var z = MathF.Pow(d.Z, 2f);
        var num = x + y + z;
        if (d.X < 0) x *= -1;
        if (d.Y < 0) y *= -1;
        if (d.Z < 0) z *= -1;
        return new Dot3(x / num, y / num, z / num);
    }
    
    public static bool InDistance(Dot3 v1, Dot3 v2, float dist) => SqrMagnitude(v1,v2) < dist * dist;

    public static bool OutDistance(Dot3 v1, Dot3 v2, float dist) => SqrMagnitude(v1,v2) > dist * dist;
    public static float SqrMagnitude(Dot3 v1, Dot3 v2) => MathF.Pow(v1.X - v2.X, 2f) + MathF.Pow(v1.Y - v2.Y, 2f) + MathF.Pow(v1.Z - v2.Z, 2f);
    
    public static Dot3 operator +(Dot3 a, Dot3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Dot3 operator -(Dot3 a, Dot3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Dot3 operator -(Dot3 a) => new(-a.X, -a.Y, -a.Z);

    public static Dot3 operator *(Dot3 a, float d) => new(a.X * d, a.Y * d, a.Z * d);

    public static Dot3 operator *(float d, Dot3 a) => new(a.X * d, a.Y * d, a.Z * d);

    public static Dot3 operator /(Dot3 a, float d) => new(a.X / d, a.Y / d, a.Z / d);

    public static bool operator ==(Dot3 lhs, Dot3 rhs)
    {
        var num1 = lhs.X - rhs.X;
        var num2 = lhs.Y - rhs.Y;
        var num3 = lhs.Z - rhs.Z;
        return num1 * (double) num1 + num2 * (double) num2 + num3 * (double) num3 < 9.999999439624929E-11;
    }

    public static bool operator !=(Dot3 lhs, Dot3 rhs) => !(lhs == rhs);
    
    public static implicit operator Dot3(Dot2 vec) => new(vec.X, vec.Y);
    public static implicit operator Dot3(Dot2Int vec) => new(vec.X, vec.Y);
    public static implicit operator Dot3(Dot3Int vec) => new(vec.X, vec.Y, vec.Z);
    public static implicit operator Dot3((float x, float y, float z) dot) => new(dot.x, dot.y, dot.z);

    public bool Clamp(Dot3 min, Dot3 max) => ClampOut(min, max) == this;

    public Dot3 ClampOut(Dot3 minInclusive, Dot3 maxInclusive) => new(MathUtils.ClampOut(X, minInclusive.X, maxInclusive.X), MathUtils.ClampOut(Y, minInclusive.Y, maxInclusive.Y), MathUtils.ClampOut(Z, minInclusive.Z, maxInclusive.Z));
    public override string ToString() => $"{{{X:F};{Y.ToString("F")};{Z.ToString("F")}}}";
  
    public bool Equals(Dot3 other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
    }

    public override bool Equals(object obj)
    {
        return obj is Dot3 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
    
}