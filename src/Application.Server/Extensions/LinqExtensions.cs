using Ingweland.Fog.Models.Fog;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Extensions;

public static class LinqExtensions
{
    public static async Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize, CancellationToken ct = default) where TDestination : class
    {
        var source = queryable.AsNoTracking();
        var count = await source.CountAsync(ct);
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        var totalPages = (int) Math.Ceiling(count / (double) pageSize);
        return new PaginatedList<TDestination>(items, count, pageNumber, totalPages);
    }
}
