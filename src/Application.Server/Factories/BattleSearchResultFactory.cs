using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class BattleSearchResultFactory(IMapper mapper) : IBattleSearchResultFactory
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

            return CreateInternal(src, src.BattleType == BattleType.Pvp, statsId);
        }).ToList();

        return new BattleSearchResult
        {
            Battles = battles,
        };
    }

    public BattleDto Create(BattleSummaryEntity entity, IReadOnlyCollection<BattleTimelineEntry> timelineEntries,
        int? statsId)
    {
        return new BattleDto
        {
            Summary = CreateInternal(entity, true, statsId),
            Timeline = timelineEntries.Select(mapper.Map<BattleTimelineEntryDto>).ToList(),
        };
    }

    private BattleSummaryDto CreateInternal(BattleSummaryEntity entity, bool addEnemySquads, int? statsId)
    {
        IReadOnlyCollection<BattleSquad>? enemySquads = null;
        if (addEnemySquads)
        {
            enemySquads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(entity.EnemySquads,
                JsonSerializerOptions) ?? [];
        }

        var playerSquads = JsonSerializer.Deserialize<IReadOnlyCollection<BattleSquad>>(entity.PlayerSquads,
            JsonSerializerOptions) ?? [];

        // This is ugly, I know. We suddenly got a Hero, which is not a hero, but just a unit...
        var playerBattleUnitDtos = playerSquads
            .Where(src => src.Hero != null && src.Hero.Properties.UnitId != "unit.Unit_SpartasLastStand_Leonidas_1")
            .OrderBy(src => src.BattlefieldSlot)
            .Select(mapper.Map<BattleSquadDto>)
            .ToList();

        var enemyBattleUnitDtos = enemySquads?
            .Where(src => src.Hero != null)
            .OrderBy(src => src.BattlefieldSlot)
            .Select(mapper.Map<BattleSquadDto>)
            .ToList() ?? [];

        return new BattleSummaryDto
        {
            Id = entity.Id,
            BattleDefinitionId = entity.BattleDefinitionId,
            ResultStatus = entity.ResultStatus,
            PlayerSquads = playerBattleUnitDtos,
            EnemySquads = enemyBattleUnitDtos,
            Difficulty = entity.Difficulty,
            StatsId = statsId,
            BattleType = entity.BattleType,
            PerformedAt = entity.PerformedAt,
        };
    }
}
