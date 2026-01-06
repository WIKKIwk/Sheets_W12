namespace W12CSheets.Client.Helpers;

/// <summary>
/// Sort helper utilities
/// </summary>
public static class SortHelper
{
    /// <summary>
    /// Bubble sort
    /// </summary>
    public static T[] BubbleSort<T>(T[] array) where T : IComparable<T>
    {
        var arr = (T[])array.Clone();
        int n = arr.Length;
        
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j].CompareTo(arr[j + 1]) > 0)
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                }
            }
        }
        
        return arr;
    }

    /// <summary>
    /// Quick sort
    /// </summary>
    public static T[] QuickSort<T>(T[] array) where T : IComparable<T>
    {
        var arr = (T[])array.Clone();
        QuickSortRecursive(arr, 0, arr.Length - 1);
        return arr;
    }

    private static void QuickSortRecursive<T>(T[] arr, int low, int high) where T : IComparable<T>
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high);
            QuickSortRecursive(arr, low, pi - 1);
            QuickSortRecursive(arr, pi + 1, high);
        }
    }

    private static int Partition<T>(T[] arr, int low, int high) where T : IComparable<T>
    {
        T pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j].CompareTo(pivot) < 0)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }

    /// <summary>
    /// Sort dictionary by value
    /// </summary>
    public static Dictionary<TKey, TValue> SortByValue<TKey, TValue>(
        Dictionary<TKey, TValue> dictionary,
        bool ascending = true) where TKey : notnull where TValue : IComparable<TValue>
    {
        var sorted = ascending
            ? dictionary.OrderBy(x => x.Value)
            : dictionary.OrderByDescending(x => x.Value);

        return sorted.ToDictionary(x => x.Key, x => x.Value);
    }
}
