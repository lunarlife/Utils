namespace Utils;

public static class EnumerableExtension
{
    public static bool AllEquals<T, T1>(this IEnumerable<T> enumerable, IEnumerable<T1> enumerable1, Func<T, T1, bool> predicate)
    {
        var arr = enumerable as T[] ?? enumerable.ToArray();
        var arr1 = enumerable1 as T1[] ?? enumerable1.ToArray();
        if (arr.Length != arr1.Length) return false;
        for (var i = 0; i < arr.Length; i++)
        {
            if (!predicate.Invoke(arr[i], arr1[i])) return false;
        }
        return true;
    }
}