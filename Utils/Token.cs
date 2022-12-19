namespace Utils;

public class Token
{
    private static readonly List<char> Chars = new();
    static Token()
    {
        for (var i = '!'; i <= '~'; i++) Chars.Add(i);
        Chars.Remove('\\');
    }

    public static string Generate(int length)
    {
        var token = "";
        var random = new Random();
        for (var i = 0; i < length; i++) token += Chars[random.Next(0, Chars.Count)];
        return token;
    }
}
