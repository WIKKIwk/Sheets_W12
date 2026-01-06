namespace W12CSheets.Client.Helpers;

/// <summary>
/// Pagination helper utilities
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Paginate collection
    /// </summary>
    public static PagedResult<T> Paginate<T>(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var list = source.ToList();
        var totalCount = list.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        var items = list
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }

    /// <summary>
    /// Get page start index
    /// </summary>
    public static int GetPageStartIndex(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize;
    }

    /// <summary>
    /// Get page end index
    /// </summary>
    public static int GetPageEndIndex(int pageNumber, int pageSize, int totalCount)
    {
        var endIndex = pageNumber * pageSize;
        return Math.Min(endIndex, totalCount);
    }

    /// <summary>
    /// Calculate total pages
    /// </summary>
    public static int CalculateTotalPages(int totalCount, int pageSize)
    {
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
