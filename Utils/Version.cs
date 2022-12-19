namespace Utils;

public readonly struct Version : IEquatable<Version>
{
    private readonly string _version;

    public Version(string version)
    {
        _version = version;
    }


    public override string ToString() => _version;

    public static implicit operator string(Version ver) => ver._version;
    public static implicit operator Version(string ver) => new(ver);

    public static bool operator ==(Version left, Version right) => left._version == right._version;

    public static bool operator !=(Version left, Version right) => !(left == right);
    
    public bool Equals(Version other) => _version == other._version;

    public override bool Equals(object obj) => obj is Version other && Equals(other);

    public override int GetHashCode() => _version.GetHashCode();
}