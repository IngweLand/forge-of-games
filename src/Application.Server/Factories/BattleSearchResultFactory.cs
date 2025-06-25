using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Application.Server.Factories;

public class BattleSearchResultFactory(IUnitService unitService, IMapper mapper) : IBattleSearchResultFactory
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()},
    };

    public async Task<BattleSearchResult> Create(IReadOnlyCollection<BattleSummaryEntity> entities,
        IReadOnlyDictionary<byte[], int> existingStatsIds)
    {
        var battles = entities.Select(src =>
        {
            int? statsId = null;
            if (existingStatsIds.TryGetValue(src.InGameBattleId, out var value))
            {
                statsId = value;
            }
            return Create(src, statsId);
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

    private BattleSummaryDto Create(BattleSummaryEntity entity, int? statsId)
    {
        var playerSquads =
            JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(entity.PlayerSquads, JsonSerializerOptions) ??
            [];
        var playerSquadDtos = playerSquads
            .Where(src => src.Hero != null)
            .OrderBy(src => src.BattlefieldSlot)
            .Select(src => mapper.Map<BattleSquadDto>(src.Hero!.Properties))
            .ToList();
        return new BattleSummaryDto
        {
            Id = entity.Id,
            BattleDefinitionId = entity.BattleDefinitionId,
            ResultStatus = entity.ResultStatus,
            PlayerSquads = playerSquadDtos,
            Difficulty = entity.Difficulty,
            StatsId = statsId,
        };
    }
}
