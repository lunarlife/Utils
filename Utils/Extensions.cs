namespace Utils;

public static class Extensions 
{
    public static T[] Copy<T>(this T[] array)
    { 
        var newArray = new T[array.Length];
        Array.Copy(array, newArray, array.Length);
        return newArray;
    }
    public static List<T> Copy<T>(this List<T> list)
    {
        var newList = new List<T>();
        newList.AddRange(list);
        return newList;
    }
}