using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.WebApp.Client.Components.Elements.StatsHub;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Elements;

public abstract class EncounterComponentBase : ComponentBase
{
    [Inject]
    private ICampaignUiService CampaignUiService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    protected async Task OnSquadClick(BattleWaveSquadViewModel squad)
    {
        if (squad.Hero == null)
        {
            return;
        }

        var fullProfile = await CampaignUiService.CreateHeroProfileAsync(squad.Hero);
        await OpenBattleSquadProfile(fullProfile);
    }

    private async Task OpenBattleSquadProfile(HeroProfileViewModel squad)
    {
        var options = GetDefaultDialogOptions();

        var parameters = new DialogParameters<ProfileSquadDialog> {{d => d.HeroProfile, squad}};
        await DialogService.ShowAsync<ProfileSquadDialog>(null, parameters, options);
    }

    private static DialogOptions GetDefaultDialogOptions()
    {
        return new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            BackgroundClass = "dialog-blur-bg",
            Position = DialogPosition.TopCenter,
            CloseButton = true,
            CloseOnEscapeKey = true,
            NoHeader = true,
        };
    }
}
