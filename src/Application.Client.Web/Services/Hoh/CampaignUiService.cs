using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
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
                Rewards = e.Rewards.SelectMany(mapper.Map<IList<EncounterRewardViewModel>>).ToList(),
                FirstTimeComletionBonus = mapper.Map<IList<EncounterRewardViewModel>>(e.FirstTimeCompletionBonus),
                Waves = e.Waves.Select((bw, bwi) =>
                    new BattleWaveViewModel
                    {
                        Title = $"~{index + 1}.{bwi + 1}",
                        Squads = bw.Squads.Select(bws => battleWaveSquadViewModelFactory.Create(bws, region.Units))
                            .ToList(),
                        AggregatedSquads = bw.Squads.GroupBy(bws => bws.UnitId)
                            .SelectMany(g => battleWaveSquadViewModelFactory.Create(g.ToList(), region.Units))
                            .ToList(),
                    }).ToList().AsReadOnly(),
                AvailableHeroSlots = new IconLabelItemViewModel()
                {
                    IconUrl = "images/icon_hud_heroes.png",
                    Label = e.AvailableHeroSlots.ToString(),
                },
            })
            .ToList().AsReadOnly();

        return new RegionViewModel
        {
            Name = $"{region.Index + 1}. {region.Name}",
            Encounters = encounters,
            Rewards = mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(region.Rewards),
        };
    }
}
