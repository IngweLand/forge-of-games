using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public string CacheBucketKey => $"Alliance_{AllianceId}";
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetAllianceQueryHandler(
    IFogDbContext context,
    IAllianceWithRankingsFactory allianceWithRankingsFactory,
    IMapper mapper)
    : IRequestHandler<GetAllianceQuery, AllianceWithRankings?>
{
    public async Task<AllianceWithRankings?> Handle(GetAllianceQuery request, CancellationToken cancellationToken)
    {
        var periodStartDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(FogConstants.DisplayedStatsDays * -1);

        var alliance = await context.Alliances.AsNoTracking()
            .Include(p => p.Members)
            .Include(p => p.NameHistory)
            .Include(p => p.Leader)
            .Include(p =>
                p.Rankings.Where(pr => pr.Type == AllianceRankingType.TotalPoints && pr.CollectedAt > periodStartDate))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.AllianceId, cancellationToken);
        if (alliance == null)
        {
            return null;
        }

        var members = alliance.Members.Where(p => p.Status == PlayerStatus.Active).OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank).ToList();

        return allianceWithRankingsFactory.Create(alliance, mapper.Map<IReadOnlyCollection<PlayerDto>>(members));
    }
}
