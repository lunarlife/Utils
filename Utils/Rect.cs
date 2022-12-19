using Utils.Dots;

namespace Utils;

public struct Rect
{
    public static readonly Rect Zero = new(0, 0, 0, 0);
    public Dot2Int Position { get; set; }
    public Dot2Int PositionWithWH => new(Position.X + Width, Position.Y + Height);
    public Dot2Int WidthHeight => new(Width, Height);
    public int Width { get; set; }
    public int Height { get; set; }

    public Rect(Dot2Int position, int width, int height)
    {
        Position = position;
        Width = width;
        Height = height;
    }
    public Rect(int x, int y, Dot2Int wh)
    {
        Position = new Dot2Int(x, y);
        Width = wh.X;
        Height = wh.Y;
    }
    public Rect(int x, int y, int width, int height)
    {
        Position = new Dot2Int(x, y);
        Width = width;
        Height = height;
    }

    public Rect(Dot2Int position, Dot2Int wh)
    {
        Position = position;
        Width = wh.X;
        Height = wh.Y;
    }
    public bool IsTouch(Rect another) =>
        DotInRect(another.Position) ||
        DotInRect(another.Position.X + another.Width, another.Position.Y) ||
        DotInRect(another.Position.X, another.Position.Y + another.Height) ||
        DotInRect(another.Position.X + another.Width, another.Position.Y + another.Height);

    public bool Inclusive(Rect another) =>
        DotInRect(another.Position) &&
        DotInRect(another.Position.X + another.Width, another.Position.Y) &&
        DotInRect(another.Position.X, another.Position.Y + another.Height) &&
        DotInRect(another.Position.X + another.Width, another.Position.Y + another.Height);

    public bool DotInRect(Dot2Int d) => DotInRect(d.X, d.Y);

    public Dot2 Insert(Rect rect)
    {
        return new Dot2Int(MathUtils.ClampOut(Position.X, rect.Position.X, rect.Position.X + rect.Width - Width), MathUtils.ClampOut(Position.Y, rect.Position.Y, rect.Position.Y + rect.Height - Height));
    }
    public bool DotInRect(int x, int y) =>
        x >= Position.X && x <= Position.X + Width && y >= Position.Y && y <= Position.Y + Height;

    public override string ToString() => $"{Position} width: {Width} height: {Height}";
    
    public static bool operator ==(Rect left, Rect right) => left.Position == right.Position && left.Width == right.Width && left.Height == right.Height;

    public static bool operator !=(Rect left, Rect right) => !(left == right);
}