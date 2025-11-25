using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Functions.Services.Interfaces;

public interface IPlayersUpdateManager : IOrchestrator
{
    Task RunAsync(IReadOnlyCollection<Player> players);
}
