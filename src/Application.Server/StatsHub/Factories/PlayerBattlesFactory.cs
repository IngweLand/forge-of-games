using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using PvpBattle = Ingweland.Fog.Models.Fog.Entities.PvpBattle;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public class PlayerBattlesFactory(IMapper mapper) : IPlayerBattlesFactory
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public PvpBattleDto Create(PvpBattle pvpBattle, int? statsId)
    {
        var winnerSquads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(pvpBattle.Teams.WinnerTeam,
            JsonSerializerOptions) ?? [];
        var loserSquads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(pvpBattle.Teams.LoserTeam,
            JsonSerializerOptions) ?? [];
        return new PvpBattleDto
        {
            Winner = mapper.Map<PlayerDto>(pvpBattle.Winner),
            Loser = mapper.Map<PlayerDto>(pvpBattle.Loser),
            WinnerUnits = mapper.Map<IReadOnlyCollection<BattleSquadDto>>(winnerSquads),
            LoserUnits = mapper.Map<IReadOnlyCollection<BattleSquadDto>>(loserSquads),
            StatsId = statsId,
        };
    }
}
