using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class CampaignUiService(
    ICampaignService campaignService,
    IBattleWaveSquadViewModelFactory battleWaveSquadViewModelFactory,
    IContinentBasicViewModelFactory continentBasicViewModelFactory,
    IAssetUrlProvider assetUrlProvider,
    IMapper mapper,
    IBattleViewModelFactory battleViewModelFactory) : ICampaignUiService
{
    private IReadOnlyCollection<ContinentBasicViewModel>? _cachedContinents;
    private IReadOnlyCollection<RegionBasicViewModel>? _cachedHistoricBattles;
    private IReadOnlyCollection<RegionBasicViewModel>? _cachedTeslaStormRegions;

    public async Task<IReadOnlyCollection<ContinentBasicViewModel>> GetCampaignContinentsBasicDataAsync()
    {
        if (_cachedContinents != null)
        {
            return _cachedContinents;
        }

        var continents = await campaignService.GetCampaignContinentsBasicDataAsync();
        _cachedContinents = continentBasicViewModelFactory.CreateContinents(continents);
        return _cachedContinents;
    }

    public async Task<IReadOnlyCollection<RegionBasicViewModel>> GetHistoricBattlesBasicDataAsync()
    {
        if (_cachedHistoricBattles != null)
        {
            return _cachedHistoricBattles;
        }

        var siegeOfOrleans = await campaignService.GetRegionBasicDataAsync(RegionId.SiegeOfOrleans);
        var spartasLastStand = await campaignService.GetRegionBasicDataAsync(RegionId.SpartasLastStand);
        var fallOfTroy = await campaignService.GetRegionBasicDataAsync(RegionId.FallOfTroy);
        _cachedHistoricBattles =
            [mapper.Map<RegionBasicViewModel>(siegeOfOrleans), mapper.Map<RegionBasicViewModel>(spartasLastStand),
                 mapper.Map<RegionBasicViewModel>(fallOfTroy)];
        return _cachedHistoricBattles;
    }

    public async Task<RegionViewModel?> GetRegionAsync(string id)
    {
        if (!Enum.TryParse<RegionId>(id, true, out var regionId))
        {
            return null;
        }

        var region = await campaignService.GetRegionAsync(regionId);
        if (region == null)
        {
            return null;
        }

        var encounters = region.Encounters
            .Select((e, index) => new EncounterViewModel
            {
                Title = (index + 1).ToString(),
                Details = e.Details.ToDictionary(kvp => kvp.Key, kvp => new EncounterDetailsViewModel
                {
                    Rewards = kvp.Value.Rewards.SelectMany(mapper.Map<IList<EncounterRewardViewModel>>).ToList(),
                    FirstTimeComletionBonus =
                        mapper.Map<IList<EncounterRewardViewModel>>(kvp.Value.FirstTimeCompletionBonus),
                    Waves = kvp.Value.Waves.Select((bw, bwi) =>
                        new BattleWaveViewModel
                        {
                            Title = $"~{index + 1}.{bwi + 1}",
                            Squads = bw.Squads.Select(bws =>
                                    battleWaveSquadViewModelFactory.Create(bws, region.Units, region.Heroes))
                                .ToList(),
                            AggregatedSquads = bw.Squads.GroupBy(bws => bws.Unit.UnitId)
                                .SelectMany(g =>
                                    battleWaveSquadViewModelFactory.Create(g.ToList(), region.Units, region.Heroes))
                                .ToList(),
                        }).ToList().AsReadOnly(),
                    AvailableHeroSlots = new IconLabelItemViewModel
                    {
                        IconUrl = assetUrlProvider.GetHohIconUrl("icon_hud_heroes"),
                        Label = kvp.Value.AvailableHeroSlots.ToString(),
                    },
                    RequiredHeroClassIconUrls =
                        kvp.Value.RequiredHeroClasses.Select(hc =>
                            assetUrlProvider.GetHohIconUrl(
                                $"{hc.GetClassIconId()}_{UnitColor.Neutral.ToString().ToLowerInvariant()}")).ToList(),
                    RequiredHeroTypeIconUrls =
                        kvp.Value.RequiredHeroTypes.Select(ht =>
                            assetUrlProvider.GetHohIconUrl(
                                $"{ht.GetTypeIconId()}_{UnitColor.Neutral.ToString().ToLowerInvariant()}")).ToList(),
                }),
            })
            .ToList().AsReadOnly();
        return new RegionViewModel
        {
            RegionId = region.Id,
            Name = $"{region.Index + 1}. {region.Name}",
            Encounters = encounters,
            Rewards = mapper.Map<IReadOnlyDictionary<Difficulty, IReadOnlyCollection<IconLabelItemViewModel>>>(
                region.Rewards),
        };
    }

    public async Task<IReadOnlyCollection<RegionBasicViewModel>> GetTeslaStormRegionsBasicDataAsync()
    {
        if (_cachedTeslaStormRegions != null)
        {
            return _cachedTeslaStormRegions;
        }

        var regions = new List<RegionId>
        {
            RegionId.TeslaStormBlue, RegionId.TeslaStormRed, RegionId.TeslaStormYellow, RegionId.TeslaStormGreen,
            RegionId.TeslaStormPurple,
        };

        var list = new List<RegionBasicViewModel>();
        foreach (var regionId in regions)
        {
            list.Add(mapper.Map<RegionBasicViewModel>(await campaignService.GetRegionBasicDataAsync(regionId)));
        }

        _cachedTeslaStormRegions = list.AsReadOnly();
        return _cachedTeslaStormRegions;
    }
    
    public Task<HeroProfileViewModel> CreateHeroProfileAsync(IBattleUnitProperties hero)
    {
        return battleViewModelFactory.CreateHeroProfileAsync(hero, null, false);
    }
}
