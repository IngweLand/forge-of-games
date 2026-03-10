using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.Heroes;

public partial class NoHeroComponent : ComponentBase
{
    private string _html = string.Empty;
    private HeroProfileViewModel? _profile;

    [Inject]
    private IHeroProfileUiService HeroProfileUiService { get; set; }

    [Inject]
    protected IJSInteropService JsInteropService { get; set; } = null!;

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    public IMarkdownSecurityService MarkdownSecurityService { get; set; }

    [Parameter]
    [EditorRequired]
    public required HeroProfileIdentifier Profile { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (OperatingSystem.IsBrowser())
        {
            _profile = await HeroProfileUiService.GetHeroProfileAsync(Profile);
            var key = $"NoHero.{Profile.HeroId}.Description";
            _html = MarkdownSecurityService.ConvertToSafeHtml(Loc[key]);
            await JsInteropService.HideLoadingIndicatorAsync();
        }
    }
}
