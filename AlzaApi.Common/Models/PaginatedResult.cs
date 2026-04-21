namespace AlzaApi.Common.Models;

public class PaginatedResult<T>(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
{
    public IEnumerable<T> Items { get; init; } = items;
    public int TotalItems { get; init; } = totalItems;

    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;

    public int TotalPages => (int) Math.Ceiling(TotalItems / (double) PageSize);
}
