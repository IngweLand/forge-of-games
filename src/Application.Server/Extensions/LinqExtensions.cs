using Ingweland.Fog.Models.Fog;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Extensions;

public static class LinqExtensions
{
    public static async Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int startIndex, int pageSize, CancellationToken ct = default)
        where TDestination : class
    {
        var source = queryable.AsNoTracking();
        var count = await source.CountAsync(ct);
        var items = await source.Skip(startIndex).Take(pageSize).ToListAsync(ct);
        return new PaginatedList<TDestination>(items, startIndex, count);
    }
}
