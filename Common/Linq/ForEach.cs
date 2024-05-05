namespace CaptainCoder.Linq;

public static class ForEachExtension
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T element in source)
        {
            action.Invoke(element);
        }
    }

    public static void ForEach<T1, T2>(this IEnumerable<T1> source, Func<T1, T2> action)
    {
        foreach (T1 element in source)
        {
            _ = action.Invoke(element);
        }
    }
}