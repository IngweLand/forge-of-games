using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetPlayerCityQuery(int PlayerId) : IRequest<HohCity?>;

public class GetPlayerCityQueryHandler(
    IFogDbContext context,
    IPlayerCityService playerCityService)
    : IRequestHandler<GetPlayerCityQuery, HohCity?>
{
    public async Task<HohCity?> Handle(GetPlayerCityQuery request, CancellationToken cancellationToken)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);
        if (player == null)
        {
            return null;
        }

        return await playerCityService.GetCityAsync(player.WorldId, player.InGamePlayerId, player.Name);
    }
}
