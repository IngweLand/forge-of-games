using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class CampaignUiService(
    ICampaignService campaignService,
    IBattleWaveSquadViewModelFactory battleWaveSquadViewModelFactory,
    IContinentBasicViewModelFactory continentBasicViewModelFactory,
    IAssetUrlProvider assetUrlProvider,
    IMapper mapper) : ICampaignUiService
{
    public async Task<IReadOnlyCollection<ContinentBasicViewModel>> GetCampaignContinentsBasicDataAsync()
    {
        var continents = await campaignService.GetCampaignContinentsBasicDataAsync();
        return continentBasicViewModelFactory.CreateContinents(continents);
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
                Details = e.Details.ToDictionary(kvp => kvp.Key, kvp => new EncounterDetailsViewModel()
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
                            AggregatedSquads = bw.Squads.GroupBy(bws => bws.UnitId)
                                .SelectMany(g =>
                                    battleWaveSquadViewModelFactory.Create(g.ToList(), region.Units, region.Heroes))
                                .ToList()
                        }).ToList().AsReadOnly(),
                    AvailableHeroSlots = new IconLabelItemViewModel()
                    {
                        IconUrl = "images/icon_hud_heroes.png",
                        Label = kvp.Value.AvailableHeroSlots.ToString()
                    },
                    RequiredHeroClassIconUrls =
                        kvp.Value.RequiredHeroClasses.Select(hc =>
                            assetUrlProvider.GetHohIconUrl(
                                $"{hc.GetClassIconId()}_{UnitColor.Neutral.ToString().ToLowerInvariant()}")).ToList(),
                    RequiredHeroTypeIconUrls =
                        kvp.Value.RequiredHeroTypes.Select(ht =>
                            assetUrlProvider.GetHohIconUrl(
                                $"{ht.GetTypeIconId()}_{UnitColor.Neutral.ToString().ToLowerInvariant()}")).ToList()
                })
            })
            .ToList().AsReadOnly();
        return new RegionViewModel
        {
            Name = $"{region.Index + 1}. {region.Name}",
            Encounters = encounters,
            Rewards =
                mapper.Map<IReadOnlyDictionary<Difficulty, IReadOnlyCollection<IconLabelItemViewModel>>>(region.Rewards)
        };
    }
}