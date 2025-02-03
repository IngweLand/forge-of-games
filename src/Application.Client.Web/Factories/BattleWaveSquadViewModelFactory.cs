using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Calculators;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BattleWaveSquadViewModelFactory(IAssetUrlProvider assetUrlProvider, IUnitPowerCalculator unitPowerCalculator, IStringLocalizer<FogResource> loc) : IBattleWaveSquadViewModelFactory
{
    public BattleWaveSquadViewModel Create(BattleWaveSquadBase squad, IReadOnlyCollection<UnitDto> units)
    {
        var size = 1;
        var unit = units.First(u => u.Id == squad.UnitId);
        var level = $"{loc[FogResource.Hoh_Lvl]} {squad.UnitLevel}";
        double power = 0;
        if (squad is BattleWaveUnitSquad unitSquad)
        {
            size = unitSquad.Size;
            power = unitPowerCalculator.CalculateUnitPower(unit, squad.UnitLevel, size);
        }

        //TODO: get concrete star class
        if (squad is BattleWaveHeroSquad heroSquad)
        {
            var abilityLvl = int.Parse(heroSquad.AbilityId[(heroSquad.AbilityId.LastIndexOf('_') + 1)..]);
            level += $" | {loc[FogResource.Hoh_Hero_AbilityLvl]} {abilityLvl}";
            power = unitPowerCalculator.CalculateHeroPower(unit,HeroStarClass.Star_2, level:squad.UnitLevel, ascensionLevel:squad.UnitLevel / 10, abilityLevel:abilityLvl);
        }

        return new BattleWaveSquadViewModel()
        {
            Name = unit.Name,
            Amount = size.ToString(),
            Color = unit.Color,
            Level = level,
            ImageUrl = assetUrlProvider.GetHohUnitPortraitUrl(unit.AssetId),
            TypeIconUrl = assetUrlProvider.GetHohIconUrl(unit.Type.GetTypeIconId()),
            IsHero = squad is BattleWaveHeroSquad,
            Power = power,
        };
    }
}
