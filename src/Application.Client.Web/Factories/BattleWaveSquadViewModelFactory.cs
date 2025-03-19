using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Calculators.Interfaces;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BattleWaveSquadViewModelFactory(
    IAssetUrlProvider assetUrlProvider,
    IUnitPowerCalculator unitPowerCalculator,
    IStringLocalizer<FogResource> loc) : IBattleWaveSquadViewModelFactory
{
    public BattleWaveSquadViewModel Create(BattleWaveSquadBase squad, IReadOnlyCollection<UnitDto> units)
    {
        var size = 1;
        if (squad is BattleWaveUnitSquad unitSquad)
        {
            size = unitSquad.Size;
        }

        return CreateInternal(squad, size, units);
    }

    public BattleWaveSquadViewModel CreateInternal(BattleWaveSquadBase squad, int squadSize,
        IReadOnlyCollection<UnitDto> units)
    {
        var unit = units.First(u => u.Id == squad.UnitId);
        var level = $"{loc[FogResource.Hoh_Lvl]} {squad.UnitLevel}";
        double power = 0;
        if (squad is BattleWaveUnitSquad)
        {
            power = unitPowerCalculator.CalculateUnitPower(unit, squad.UnitLevel, squadSize);
        }

        //TODO: get concrete star class
        if (squad is BattleWaveHeroSquad heroSquad)
        {
            var abilityLvl = int.Parse(heroSquad.AbilityId[(heroSquad.AbilityId.LastIndexOf('_') + 1)..]);
            level += $" | {loc[FogResource.Hoh_Hero_AbilityLvl]} {abilityLvl}";
            power = unitPowerCalculator.CalculateHeroPower(unit, HeroStarClass.Star_2, level: squad.UnitLevel,
                ascensionLevel: squad.UnitLevel / 10, abilityLevel: abilityLvl);
        }

        UnitColorAffinity? colorAffinity = null;
        if (UnitColorAdvantageSystem.ColorAffinities.TryGetValue(unit.Color, out var unitColorAffinity))
        {
            colorAffinity = unitColorAffinity;
        }
        
        return new BattleWaveSquadViewModel()
        {
            Name = unit.Name,
            Amount = squadSize.ToString(),
            Color = unit.Color,
            Level = level,
            ImageUrl = assetUrlProvider.GetHohUnitPortraitUrl(unit.AssetId),
            TypeIconUrl = assetUrlProvider.GetHohIconUrl(unit.Type.GetTypeIconId()),
            IsHero = squad is BattleWaveHeroSquad,
            Power = power,
            ColorAffinity = colorAffinity,
        };
    }

    public IEnumerable<BattleWaveSquadViewModel> Create(IReadOnlyCollection<BattleWaveSquadBase> squads,
        IReadOnlyCollection<UnitDto> units)
    {
        if (squads == null || squads.Count == 0)
        {
            throw new ArgumentException("Squads collection cannot be null or empty.", nameof(squads));
        }

        var firstSquad = squads.First();
        var expectedUnitId = firstSquad.UnitId;
        if (squads.Any(s => s.UnitId != expectedUnitId))
        {
            throw new InvalidOperationException($"All squads must have the same {nameof(BattleWaveSquadBase.UnitId)}.");
        }
        
        var result = new List<BattleWaveSquadViewModel>();

        switch (firstSquad)
        {
            case BattleWaveUnitSquad:
            {
                var total = squads.OfType<BattleWaveUnitSquad>().Sum(s => s.Size);
                result.Add(CreateInternal(firstSquad, total, units));
                break;
            }
            case BattleWaveHeroSquad:
                result.AddRange(squads.Select(squad => CreateInternal(squad, 1, units)));
                break;
        }

        return result;
    }
}