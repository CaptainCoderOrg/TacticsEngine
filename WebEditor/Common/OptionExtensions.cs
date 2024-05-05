using Optional;

namespace CaptainCoder.Optional;

public static class OptionExtensions
{
    public static void ForEach<T>(this Option<T> source, Action<T> action)
    {
        foreach (T element in source)
        {
            action.Invoke(element);
        }
    }

    public static void ForEach<T1, T2>(this Option<T1> source, Func<T1, T2> action)
    {
        foreach (T1 element in source)
        {
            _ = action.Invoke(element);
        }
    }

}