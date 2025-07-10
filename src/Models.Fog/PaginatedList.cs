using System.Text.Json.Serialization;

namespace Ingweland.Fog.Models.Fog;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int startIndex, int totalCount)
{
    public static readonly PaginatedList<T> Empty = new([], 0, 0);
    public IReadOnlyCollection<T> Items { get; } = items;
    /// <summary>
    /// The index in the full dataset where this batch starts (zero-based).
    /// </summary>
    public int StartIndex { get; } = startIndex;

    /// <summary>
    /// The number of items in this batch.
    /// </summary>
    [JsonIgnore]
    public int Count { get; } = items.Count;

    /// <summary>
    /// The total number of items available in the full dataset.
    /// </summary>
    public int TotalCount { get; } = totalCount;

    [JsonIgnore]
    public bool HasMore => StartIndex + Count < TotalCount;
}
