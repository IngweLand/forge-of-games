using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.Application.Server.Factories;

public class BattleSearchResultFactory(IUnitService unitService, IMapper mapper) : IBattleSearchResultFactory
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public async Task<BattleSearchResult> Create(IReadOnlyCollection<BattleSummaryEntity> entities,
        IReadOnlyDictionary<byte[], int> existingStatsIds, BattleSquadSide side = BattleSquadSide.Player)
    {
        var battles = entities.Select(src =>
        {
            int? statsId = null;
            if (existingStatsIds.TryGetValue(src.InGameBattleId, out var value))
            {
                statsId = value;
            }

            return Create(src, statsId, side);
        }).ToList();
        var heroIds = battles.SelectMany(src => src.PlayerSquads.Select(s => s.UnitId)).ToHashSet();
        var heroTasks = heroIds.Select(unitService.GetHeroAsync);
        var heroes = await Task.WhenAll(heroTasks);
        return new BattleSearchResult
        {
            Battles = battles,
            Heroes = heroes!,
        };
    }

    private BattleSummaryDto Create(BattleSummaryEntity entity, int? statsId, BattleSquadSide side)
    {
        IReadOnlyCollection<BattleSquad> squads;
        if (side == BattleSquadSide.Enemy)
        {
            squads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(entity.EnemySquads,
                JsonSerializerOptions) ?? [];
        }
        else
        {
            squads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(entity.PlayerSquads,
                JsonSerializerOptions) ?? [];
        }

        var playerBattleUnitDtos = squads
            .Where(src => src.Hero != null)
            .OrderBy(src => src.BattlefieldSlot)
            .Select(src => mapper.Map<BattleUnitDto>(src.Hero!.Properties))
            .ToList();
        return new BattleSummaryDto
        {
            Id = entity.Id,
            BattleDefinitionId = entity.BattleDefinitionId,
            ResultStatus = side == BattleSquadSide.Player ? entity.ResultStatus : entity.ResultStatus.Reverse(),
            PlayerSquads = playerBattleUnitDtos,
            Difficulty = entity.Difficulty,
            StatsId = statsId,
        };
    }
}
