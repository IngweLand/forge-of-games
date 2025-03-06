using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllianceQuery : IRequest<AllianceWithRankings?>
{
    public required int AllianceId { get; init; }
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
            .Include(p => p.MemberHistory)
            .Include(p => p.NameHistory)
            .Include(p => p.Leader)
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > periodStartDate))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.AllianceId, cancellationToken: cancellationToken);
        if (alliance == null)
        {
            return null;
        }

        var members = alliance.Members.OrderByDescending(p => p.RankingPoints).ThenBy(p => p.Rank).ToList();
        var memberIds = members.Select(p => p.Id).ToHashSet();
        var possibleMembers = await context.Players.AsNoTracking()
            .Where(p => p.AllianceName == alliance.Name && !memberIds.Contains(p.Id))
            .OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

        return allianceWithRankingsFactory.Create(alliance, mapper.Map<IReadOnlyCollection<PlayerDto>>(members),
            possibleMembers);
    }
}