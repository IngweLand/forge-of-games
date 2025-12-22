using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.PlayerCity.Queries;

public record GetPlayerEventCityQuery(int PlayerId) : IRequest<HohCity?>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetPlayerEventCityQueryHandler(
    IFogDbContext context,
    IPlayerCityService playerCityService,
    IHohCityCreationService cityCreationService,
    IFailedPlayerCityFetchesCache failedFetchesCache,
    ISender sender)
    : IRequestHandler<GetPlayerEventCityQuery, HohCity?>
{
    public async Task<HohCity?> Handle(GetPlayerEventCityQuery request, CancellationToken cancellationToken)
    {
        var player = await context.Players.FirstOrDefaultAsync(p => p.Id == request.PlayerId, cancellationToken);
        if (player == null)
        {
            return null;
        }

        var inGameEvent = await sender.Send(new GetCurrentInGameEventQuery(player.WorldId, EventDefinitionId.EventCity),
            cancellationToken);
        if (inGameEvent == null)
        {
            return null;
        }

        var wonderId = Enum.GetValues<WonderId>()
            .FirstOrDefault(x => inGameEvent.InGameDefinitionId.EndsWith(x.ToString()));
        if (wonderId == WonderId.Undefined)
        {
            return null;
        }

        if (failedFetchesCache.IsFailedFetch(player.Key))
        {
            return null;
        }

        var fetchedCity =
            await playerCityService.FetchCityAsync(player.WorldId, player.InGamePlayerId, wonderId.ToCity());
        if (fetchedCity == null)
        {
            failedFetchesCache.AddFailedFetch(player.Key);
            return null;
        }

        return await cityCreationService.Create(fetchedCity, player.Name);
    }
}
