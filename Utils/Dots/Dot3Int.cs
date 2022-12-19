namespace Utils.Dots;

public struct Dot3Int : IEquatable<Dot3Int>
{
    public int X;
    public int Y;
    public int Z;
 
    
    public Dot3Int(int x, int y)
    {
        this.X = x;
        this.Y = y;
        Z = 0;
    }

    public Dot3Int(int x, int y, int z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }
    
    
    public static Dot3Int operator +(Dot3Int a, Dot3Int b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Dot3Int operator -(Dot3Int a, Dot3Int b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Dot3Int operator *(Dot3Int a, Dot3Int b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    public static Dot3Int operator -(Dot3Int a) => new(-a.X, -a.Y, -a.Z);

    public static Dot3Int operator *(Dot3Int a, int b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Dot3Int operator *(int a, Dot3Int b) => new(a * b.X, a * b.Y, a * b.Z);

    public static Dot3Int operator /(Dot3Int a, int b) => new(a.X / b, a.Y / b, a.Z / b);

    public static bool operator ==(Dot3Int lhs, Dot3Int rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

    public static bool operator !=(Dot3Int lhs, Dot3Int rhs) => !(lhs == rhs);
    
    public static implicit operator Dot3Int(Dot2 vec) => new((int)vec.X, (int)vec.Y);
    public static implicit operator Dot3Int(Dot2Int vec) => new(vec.X, vec.Y);
    public static implicit operator Dot3Int(Dot3 vec) => new((int)vec.X, (int)vec.Y, (int)vec.Z);
    public static implicit operator Dot3Int((int x, int y, int z) dot) => new(dot.x, dot.y, dot.z);

    public override string ToString() => $"{{{X};{Y};{Z}}}";
    public bool Equals(Dot3Int other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }

    public override bool Equals(object obj)
    {
        return obj is Dot3Int other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}