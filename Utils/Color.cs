namespace Utils
{
    public struct Color : IEquatable<Color>
    {
        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        public override bool Equals(object obj)
        {
            return obj is Color other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B, A);
        }

        public static Color Pink { get; } = new(255, 20, 147);
        public static Color Orange { get; } = new(255, 140, 0);
        public static Color Gold { get; } = new(255, 215, 0);
        public static Color Yellow { get; } = new(255, 255, 0);
        public static Color Magenta { get; } = new(255, 0, 255);
        public static Color Aqua { get; } = new(0, 255, 255);
        public static Color Blue { get; } = new(0, 0, 255);
        public static Color DarkBlue { get; } = new(0, 0, 139);
        public static Color Brown { get; } = new(165, 42, 42);
        public static Color White { get; } = new(255, 255, 255);
        public static Color Gray { get; } = new(128, 128, 128);
        public static Color Black { get; } = new(0, 0, 0);
        public static Color DarkGray { get; } = new(105, 105, 105);
        public static Color Red { get; } = new(255, 0, 0);
        public static Color DarkRed { get; } = new(139, 0, 0);
        public static Color OrangeRed { get; } = new(255, 69, 0);
        public static Color Transparent { get; } = new(0, 0, 0, 0);

        
        public byte R { get; set; } = 255;
        public byte G { get; set; } = 255;
        public byte B { get; set; } = 255;
        public byte A { get; set; } = 255;

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color Alpha(byte a) => this with { A = a };
        public static bool operator ==(Color left, Color right) => left.R == right.R && left.G == right.G && left.B == right.B && left.A == right.A;

        public static bool operator !=(Color left, Color right) => !(left == right);
    }
}