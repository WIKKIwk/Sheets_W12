namespace W12CSheets.Client.Helpers;

/// <summary>
/// Search helper utilities
/// </summary>
public static class SearchHelper
{
    /// <summary>
    /// Linear search
    /// </summary>
    public static int LinearSearch<T>(T[] array, T target) where T : IEquatable<T>
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(target))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Binary search (array must be sorted)
    /// </summary>
    public static int BinarySearch<T>(T[] array, T target) where T : IComparable<T>
    {
        int left = 0;
        int right = array.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int comparison = array[mid].CompareTo(target);

            if (comparison == 0)
                return mid;
            
            if (comparison < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return -1;
    }

    /// <summary>
    /// Find all occurrences
    /// </summary>
    public static List<int> FindAll<T>(T[] array, T target) where T : IEquatable<T>
    {
        var indices = new List<int>();
        
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(target))
            {
                indices.Add(i);
            }
        }
        
        return indices;
    }

    /// <summary>
    /// Search in collection with predicate
    /// </summary>
    public static T? Find<T>(IEnumerable<T> collection, Func<T, bool> predicate)
    {
        return collection.FirstOrDefault(predicate);
    }

    /// <summary>
    /// Find all matching items
    /// </summary>
    public static List<T> FindAll<T>(IEnumerable<T> collection, Func<T, bool> predicate)
    {
        return collection.Where(predicate).ToList();
    }

    /// <summary>
    /// Check if any item matches predicate
    /// </summary>
    public static bool Any<T>(IEnumerable<T> collection, Func<T, bool> predicate)
    {
        return collection.Any(predicate);
    }

    /// <summary>
    /// Check if all items match predicate
    /// </summary>
    public static bool All<T>(IEnumerable<T> collection, Func<T, bool> predicate)
    {
        return collection.All(predicate);
    }
}
