using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcHeroPage : CcProfilePageBase
{
    private HeroProfileIdentifier? _heroProfileIdentifier;

    [Inject]
    private ICcProfileUiService CcProfileUiService { get; set; }

    protected override async Task HandleOnParametersSetAsync()
    {
        await base.HandleOnParametersSetAsync();

        _heroProfileIdentifier = await CcProfileUiService.GetHeroProfileIdentifierAsync(HeroId);
    }

    private async Task SaveProfile(HeroProfileIdentifier identifier)
    {
         await CcProfileUiService.UpdateHeroProfileAsync(identifier);
    }
}
