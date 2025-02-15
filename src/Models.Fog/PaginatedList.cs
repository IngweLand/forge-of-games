using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int totalPages)
{
    [JsonIgnore]
    public bool HasNextPage => PageNumber < TotalPages;

    [JsonIgnore]
    public bool HasPreviousPage => PageNumber > 1;

    public IReadOnlyCollection<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int TotalCount { get; } = totalCount;
    public int TotalPages { get; } = totalPages;
}
