using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IInGameEventUiService
{
    Task<WonderId> GetCurrentWonderAsync(string worldId, CancellationToken ct = default);
}
