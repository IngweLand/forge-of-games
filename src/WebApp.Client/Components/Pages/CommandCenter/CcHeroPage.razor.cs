using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcHeroPage : CommandCenterPageBase
{
    private HeroProfileViewModel? _heroProfileViewModel;
    private IReadOnlyCollection<IconLabelItemViewModel>? _progressionCost;

    [Inject]
    private ICcHeroesPlaygroundUiService PlaygroundUiService { get; set; }

    [Inject]
    private ICcProfileUiService ProfileUiService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (string.IsNullOrWhiteSpace(ProfileId))
        {
            _heroProfileViewModel = await PlaygroundUiService.GetHeroProfileAsync(HeroProfileId);
        }
        else
        {
            _heroProfileViewModel = await ProfileUiService.GetHeroProfileAsync(ProfileId, HeroProfileId);
        }
    }

    private void OnProgressionTargetLevelChanged(HeroProgressionCostRequest request)
    {
        _progressionCost = CommandCenterUiService.CalculateHeroProgressionCost(request);
    }

    private void UpdateProfile(HeroProfileStatsUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(ProfileId))
        {
            _heroProfileViewModel = PlaygroundUiService.UpdateHeroProfile(request);
        }
        else
        {
            _heroProfileViewModel = ProfileUiService.UpdateHeroProfile(request);
        }
    }
}
