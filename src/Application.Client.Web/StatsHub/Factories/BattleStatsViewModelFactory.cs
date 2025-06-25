using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class BattleStatsViewModelFactory(IAssetUrlProvider assetUrlProvider) : IBattleStatsViewModelFactory
{
    public BattleStatsViewModel Create(BattleStatsDto statsDto)
    {
        return new BattleStatsViewModel
        {
            PlayerSquads = CreateSquad(statsDto.PlayerSquads),
            EnemySquads = CreateSquad(statsDto.EnemySquads),
        };
    }

    private IReadOnlyCollection<BattleStatsSquadViewModel> CreateSquad(
        IReadOnlyCollection<BattleSquadStatsDto> squadStats)
    {
        return squadStats.Select(dto => new BattleStatsSquadViewModel
        {
            HeroPortraitUrl = dto.Hero != null
                ? assetUrlProvider.GetHohUnitPortraitUrl(dto.Hero.AssetId)
                : null,
            SupportUnitPortraitUrl = dto.SupportUnit != null
                ? assetUrlProvider.GetHohUnitPortraitUrl(dto.SupportUnit.AssetId)
                : null,
            HeroName = dto.Hero?.Name,
            SupportUnitName = dto.SupportUnit?.Name,
            AttackValue = CreateValue(dto.Hero?.Attack, dto.SupportUnit?.Attack),
            DefenseValue = CreateValue(dto.Hero?.Defense, dto.SupportUnit?.Defense),
            HealValue = CreateValue(dto.Hero?.Heal, dto.SupportUnit?.Heal),
        }).ToList();
    }

    private BattleStatsValueViewModel CreateValue(float? heroValue, float? supportUnitValue)
    {
        var total = heroValue ?? 0;
        total += supportUnitValue ?? 0;
        return new BattleStatsValueViewModel
        {
            HeroValue = heroValue.HasValue ? ((int) heroValue).ToString() : "-",
            SupportUnitValue = supportUnitValue.HasValue ? ((int) supportUnitValue).ToString() : "-",
            TotalValue = ((int) total).ToString(),
        };
    }
}
