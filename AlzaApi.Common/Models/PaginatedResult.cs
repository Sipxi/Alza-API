namespace AlzaApi.Common.Models;

public class PaginatedResult<T>(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
{
    public IEnumerable<T> Items { get; set; } = items;
    public int TotalItems { get; set; } = totalItems;

    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;

    public int TotalPages => (int) Math.Ceiling(TotalItems / (double) PageSize);
}
