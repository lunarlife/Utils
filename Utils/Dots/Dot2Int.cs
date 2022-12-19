namespace Utils.Dots;

public struct Dot2Int : IEquatable<Dot2Int>
{
    public int X;
    public int Y;
    
    public static readonly Dot2Int Zero = new(0, 0); 
    public static readonly Dot2Int One = new(1, 1); 
    public static readonly Dot2Int MinusOne = new(-1, -1); 
    public static readonly Dot2Int Left = new(-1, 0); 
    public static readonly Dot2Int Right = new(1, 0); 
    public static readonly Dot2Int Up = new(0, 1); 
    public static readonly Dot2Int Down = new(0, -1); 
    
    public Dot2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Clamp(Dot2Int min, Dot2Int max) => ClampOut(min, max) == this;

    public Dot2Int ClampOut(Dot2Int minInclusive, Dot2Int maxInclusive) => new(MathUtils.ClampOut(X, minInclusive.X, maxInclusive.X), MathUtils.ClampOut(Y, minInclusive.Y, maxInclusive.Y));

    public static Dot2Int operator +(Dot2Int a, Dot2Int b) => new(a.X + b.X, a.Y + b.Y);

    public static Dot2Int operator -(Dot2Int a, Dot2Int b) => new(a.X - b.X, a.Y - b.Y);

    public static Dot2Int operator *(Dot2Int a, Dot2Int b) => new(a.X * b.X, a.Y * b.Y);

    public static Dot2Int operator /(Dot2Int a, Dot2Int b) => new(a.X / b.X, a.Y / b.Y);

    public static Dot2Int operator -(Dot2Int a) => new(-a.X, -a.Y);

    public static Dot2Int operator *(Dot2Int a, int d) => new(a.X * d, a.Y * d);

    public static Dot2Int operator *(int d, Dot2Int a) => new(a.X * d, a.Y * d);

    public static Dot2Int operator /(Dot2Int a, int d) => new(a.X / d, a.Y / d);

    public static Dot2 operator *(Dot2Int a, float d) => new(a.X * d, a.Y * d);

    public static Dot2 operator *(float d, Dot2Int a) => new(a.X * d, a.Y * d);

    public static Dot2 operator /(Dot2Int a, float d) => new(a.X / d, a.Y / d);
    
    public static Dot2Int operator +(Dot2Int a, float b) => new(a.X + (int)b, a.Y + (int)b);

    public static Dot2 operator -(Dot2Int a, float b) => new(a.X - (int)b, a.Y - (int)b);
    
    public static Dot2Int operator +(Dot2Int a, int b) => new(a.X + b, a.Y + b);

    public static Dot2Int operator -(Dot2Int a, int b) => new(a.X - b, a.Y - b);
    
    
    public static bool operator ==(Dot2Int lhs, Dot2Int rhs)
    {
        var num1 = lhs.X - rhs.X;
        var num2 = lhs.Y - rhs.Y;
        return num1 * (double) num1 + num2 * (double) num2 < 9.999999439624929E-11;
    }
    public static bool operator !=(Dot2Int lhs, Dot2Int rhs) => !(lhs == rhs);

    public static implicit operator Dot2Int(Dot2 dot) => new((int)dot.X, (int)dot.Y);
    public static implicit operator Dot2Int((int x, int y) dot) => new(dot.x, dot.y);
    public override string ToString() => $"{{{X};{Y}}}";
  
    public bool Equals(Dot2Int other) => X.Equals(other.X) && Y.Equals(other.Y);

    public override bool Equals(object obj) => obj is Dot2Int other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);
}