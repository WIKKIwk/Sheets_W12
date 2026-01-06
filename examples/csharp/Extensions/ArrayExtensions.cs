namespace W12CSheets.Client.Extensions;

/// <summary>
/// Extension methods for arrays
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Shuffle array
    /// </summary>
    public static T[] Shuffle<T>(this T[] array)
    {
        var random = new Random();
        var result = (T[])array.Clone();
        
        for (int i = result.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (result[i], result[j]) = (result[j], result[i]);
        }
        
        return result;
    }

    /// <summary>
    /// Get random element
    /// </summary>
    public static T? RandomElement<T>(this T[] array)
    {
        if (array.Length == 0) return default;
        var random = new Random();
        return array[random.Next(array.Length)];
    }

    /// <summary>
    /// Chunk array
    /// </summary>
    public static T[][] Chunk<T>(this T[] array, int chunkSize)
    {
        var chunks = new List<T[]>();
        
        for (int i = 0; i < array.Length; i += chunkSize)
        {
            var chunk = array.Skip(i).Take(chunkSize).ToArray();
            chunks.Add(chunk);
        }
        
        return chunks.ToArray();
    }

    /// <summary>
    /// Fill array with value
    /// </summary>
    public static void Fill<T>(this T[] array, T value)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = value;
        }
    }

    /// <summary>
    /// Rotate array left
    /// </summary>
    public static T[] RotateLeft<T>(this T[] array, int positions)
    {
        var result = new T[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = array[(i + positions) % array.Length];
        }
        return result;
    }

    /// <summary>
    /// Rotate array right
    /// </summary>
    public static T[] RotateRight<T>(this T[] array, int positions)
    {
        return RotateLeft(array, array.Length - positions);
    }
}
