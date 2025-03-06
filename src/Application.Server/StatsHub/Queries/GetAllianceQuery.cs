using System.Diagnostics;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllianceQuery : IRequest<AllianceWithRankings?>
{
    public required AllianceKey AllianceKey { get; init; }
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
            .Include(p => p.Rankings.Where(pr => pr.CollectedAt > periodStartDate))
            .FirstOrDefaultAsync(
                p => p.WorldId == request.AllianceKey.WorldId &&
                     p.InGameAllianceId == request.AllianceKey.InGameAllianceId,
                cancellationToken: cancellationToken);
        if (alliance == null)
        {
            return null;
        }
        
        var members = await context.Players.AsNoTracking()
            .Where(p => p.AllianceName == alliance.Name)
            .OrderByDescending(p => p.RankingPoints)
            .ThenBy(p => p.Rank)
            .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
        
        var allMemberKeys = await context.PlayerRankings.AsNoTracking()
            .Where(p => p.AllianceName == alliance.Name)
            .ProjectTo<PlayerKey>(mapper.ConfigurationProvider)
            .ToHashSetAsync(cancellationToken: cancellationToken);
        allMemberKeys.ExceptWith(members.Select(p => p.Key).ToHashSet());
        List<PlayerDto> possiblePastMembers = [];
        if (allMemberKeys.Count > 0)
        {
            var inGamePlayerIds = allMemberKeys.Select(k => k.InGamePlayerId).ToHashSet();
            possiblePastMembers = await context.Players.AsNoTracking()
                .Where(p => inGamePlayerIds.Contains(p.InGamePlayerId))
                .OrderByDescending(p => p.RankingPoints)
                .ThenBy(p => p.Rank)
                .ProjectTo<PlayerDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);
            possiblePastMembers = possiblePastMembers.Where(p => p.Key.WorldId == request.AllianceKey.WorldId).ToList();
        }
        
        return allianceWithRankingsFactory.Create(alliance, members, possiblePastMembers);
    }
}