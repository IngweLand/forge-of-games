namespace Ingweland.Fog.Functions.Extensions;

public static class HistoryServiceExtensions
{
    public static Dictionary<int, List<(int Id, T Value, DateTime CollectedAt)>> Aggregate<T>(
        this IEnumerable<(int Id, T Value, DateTime CollectedAt)> allItems)
    {
        return allItems
            .OrderBy(t => t.CollectedAt)
            .Aggregate(new Dictionary<int, List<(int Id, T Value, DateTime CollectedAt)>>(), (acc, t) =>
            {
                if (acc.TryGetValue(t.Id, out var list))
                {
                    if (!EqualityComparer<T>.Default.Equals(list.Last().Value, t.Value))
                    {
                        list.Add(t);
                    }
                }
                else
                {
                    acc[t.Id] = [t];
                }

                return acc;
            });
    }

    public static List<(T Value, DateTime CollectedAt)> Aggregate<T>(
        this IEnumerable<(T Value, DateTime CollectedAt)> items)
    {
        return items
            .OrderBy(t => t.CollectedAt)
            .Aggregate(new List<(T Value, DateTime CollectedAt)>(), (acc, t) =>
            {
                if (acc.Count == 0 || !EqualityComparer<T>.Default.Equals(acc.Last().Value, t.Value))
                {
                    acc.Add(t);
                }

                return acc;
            });
    }
}