using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ingweland.Fog.Infrastructure.Converters;

public class SetValueComparer<T>() : ValueComparer<ISet<T>>((c1, c2) => c1!.SetEquals(c2!),
    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
    c => c.ToHashSet());

public class CollectionValueComparer<T>() : ValueComparer<ICollection<T>>((c1, c2) => c1!.SequenceEqual(c2!),
    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
    c => c.ToHashSet());
