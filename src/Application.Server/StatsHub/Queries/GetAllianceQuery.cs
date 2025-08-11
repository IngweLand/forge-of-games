using AutoMapper;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllianceQuery : IRequest<AllianceWithRankings?>, ICacheableRequest
{
    public required int AllianceId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetAllianceQueryHandler(
    IFogDbContext context,
    IAllianceWithRankingsFactory allianceWithRankingsFactory)
    : IRequestHandler<GetAllianceQuery, AllianceWithRankings?>
{
    public async Task<AllianceWithRankings?> Handle(GetAllianceQuery request, CancellationToken cancellationToken)
    {
        var periodStartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(FogConstants.DisplayedStatsDays * -1);

        var alliance = await context.Alliances.AsNoTracking()
            .Include(p => p.Members.Where(x => x.Player.Status == InGameEntityStatus.Active)).ThenInclude(x => x.Player)
            .Include(p => p.NameHistory)
            .Include(p =>
                p.Rankings.Where(pr => pr.Type == AllianceRankingType.TotalPoints && pr.CollectedAt > periodStartDate))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.AllianceId, cancellationToken);
        return alliance == null ? null : allianceWithRankingsFactory.Create(alliance);
    }
}
