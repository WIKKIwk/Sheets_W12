namespace W12CSharp.Client.Extensions;

/// <summary>
/// Extension methods for collections
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Check if collection is null or empty
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? collection)
    {
        return collection == null || !collection.Any();
    }

    /// <summary>
    /// Chunk collection into batches
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
    {
        if (chunkSize <= 0)
            throw new ArgumentException("Chunk size must be greater than zero", nameof(chunkSize));

        var list = source.ToList();
        for (int i = 0; i < list.Count; i += chunkSize)
        {
            yield return list.Skip(i).Take(chunkSize);
        }
    }

    /// <summary>
    /// Shuffle collection randomly
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var random = new Random();
        return source.OrderBy(x => random.Next());
    }

    /// <summary>
    /// Get distinct items by key
    /// </summary>
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    /// <summary>
    /// ForEach for IEnumerable
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// To dictionary safely (handles duplicate keys)
    /// </summary>
    public static Dictionary<TKey, TValue> ToSafeDictionary<TSource, TKey, TValue>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TValue> valueSelector) where TKey : notnull
    {
        var dictionary = new Dictionary<TKey, TValue>();
        foreach (var item in source)
        {
            var key = keySelector(item);
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = valueSelector(item);
            }
        }
        return dictionary;
    }

    /// <summary>
    /// Get random element from collection
    /// </summary>
    public static T? Random<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        if (list.Count == 0) return default;
        
        var random = new Random();
        return list[random.Next(list.Count)];
    }

    /// <summary>
    /// Get random elements from collection
    /// </summary>
    public static IEnumerable<T> RandomElements<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    /// <summary>
    /// Partition collection by predicate
    /// </summary>
    public static (IEnumerable<T> Matches, IEnumerable<T> NonMatches) Partition<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        var list = source.ToList();
        return (list.Where(predicate), list.Where(x => !predicate(x)));
    }
}
