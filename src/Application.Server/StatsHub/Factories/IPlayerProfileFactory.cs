using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public interface IPlayerProfileFactory
{
    PlayerProfile Create(Player player, IReadOnlyCollection<PvpBattle> pvpBattles,
        IReadOnlyDictionary<byte[], int> existingStatsIds);
}
