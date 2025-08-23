using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Ingweland.Fog.Application.Server.Battle.Queries;

public record BattleSearchQuery : IRequest<BattleSearchResult>, ICacheableRequest
{
    public required string BattleDefinitionId { get; init; }
    public required BattleType BattleType { get; init; }
    public IReadOnlyCollection<string> UnitIds { get; init; } = new List<string>();
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class BattleSearchQueryHandler(
    IBattleSearchResultFactory battleSearchResultFactory,
    IFogDbContext context,
    IBattleQueryService battleQueryService,
    ILogger<BattleSearchQueryHandler> logger)
    : IRequestHandler<BattleSearchQuery, BattleSearchResult>
{
    public async Task<BattleSearchResult> Handle(BattleSearchQuery request,
        CancellationToken cancellationToken)
    {
        Stopwatch? sw = null;
        TimeSpan last = TimeSpan.Zero;
        if (logger.IsEnabled(LogLevel.Debug))
        {
            sw = Stopwatch.StartNew();
            logger.LogDebug(
                "Handling request DefinitionId={DefinitionId}, Type={BattleType}, UnitCount={UnitCount}",
                request.BattleDefinitionId, request.BattleType, request.UnitIds.Count);
        }

        IQueryable<BattleSummaryEntity> battlesQuery;
        if (request.UnitIds.Count > 0)
        {
            var unitIds = request.UnitIds.ToHashSet();
            battlesQuery = BuildBattleQuery(unitIds, request.BattleDefinitionId, request.BattleType);
        }
        else
        {
            battlesQuery = context.Battles.AsNoTracking()
                .Where(src => src.BattleDefinitionId == request.BattleDefinitionId);
        }

        if (sw is not null)
        {
            var now = sw.Elapsed;
            logger.LogDebug("Built query in {StepMs} ms (total {TotalMs} ms)",
                (now - last).TotalMilliseconds, now.TotalMilliseconds);
            last = now;
        }

        var battles = await battlesQuery
            .OrderByDescending(src => src.PerformedAt)
            .Take(FogConstants.MaxDisplayedBattles)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        if (sw is not null)
        {
            var now = sw.Elapsed;
            logger.LogDebug("Fetched {BattleCount} battles in {StepMs} ms (total {TotalMs} ms)",
                battles.Count, (now - last).TotalMilliseconds, now.TotalMilliseconds);
            last = now;
        }

        var battleIds = battles.Select(src => src.InGameBattleId);

        var existingStatsIds = await battleQueryService.GetExistingBattleStatsIdsAsync(battleIds, cancellationToken);

        if (sw is not null)
        {
            var now = sw.Elapsed;
            logger.LogDebug("Retrieved existing battle stats ids in {StepMs} ms (total {TotalMs} ms)",
                (now - last).TotalMilliseconds, now.TotalMilliseconds);
            last = now;
        }

        var result = await battleSearchResultFactory.Create(battles, existingStatsIds);

        if (sw is not null)
        {
            var now = sw.Elapsed;
            logger.LogDebug("Created result in {StepMs} ms (total {TotalMs} ms)",
                (now - last).TotalMilliseconds, now.TotalMilliseconds);
            logger.LogDebug("Completed in {TotalMs} ms", now.TotalMilliseconds);
        }

        return result;
    }

    private IQueryable<BattleSummaryEntity> BuildBattleQuery(HashSet<string> unitIds, string battleDefinitionId,
        BattleType battleType)
    {
        if (battleType == BattleType.Pvp)
        {
            return context.Battles.AsNoTracking()
                .Where(b => b.BattleDefinitionId == battleDefinitionId &&
                    unitIds.All(requiredId => b.Units.Any(u => u.UnitId == requiredId)));
        }

        return context.Battles.AsNoTracking()
            .Where(b => b.BattleDefinitionId == battleDefinitionId &&
                unitIds.All(requiredId =>
                    b.Units.Any(u => u.UnitId == requiredId && u.Side == BattleSquadSide.Player)));
    }
}