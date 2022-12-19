namespace Utils;

public static class ReflectionUtils
{
    public static Type? FindType(string type) => AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetTypes().FirstOrDefault(f => f.FullName == type)).FirstOrDefault();
}