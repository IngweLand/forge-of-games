using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.Heroes;

public partial class HeroComponent : ComponentBase
{
    private Dictionary<string, object> _defaultAnalyticsParameters = [];
    private HeroProfileIdentifier? _initProfile;
    private bool _isVideoAvatarLoadFailed;
    private HeroProfileViewModel? _profile;
    private IReadOnlyCollection<IconLabelItemViewModel>? _progressionCost;
    private HeroLevelSpecs? _progressionTargetLevel;
    private IList<HeroLevelSpecs>? _progressionTargetLevels;
    private bool _showVideoAvatar;

    [Parameter]
    public string AnalyticsLocation { get; set; } = AnalyticsParams.Values.Locations.HERO_COMPONENT;

    [Inject]
    private IHeroComponentAnalyticsService AnalyticsService { get; set; }

    [Inject]
    private IHeroProfileIdentifierFactory HeroProfileIdentifierFactory { get; set; }

    [Inject]
    private IHeroProfileUiService HeroProfileUiService { get; set; }

    [Parameter]
    [EditorRequired]
    public required HeroProfileIdentifier InitProfile { get; set; }

    [Inject]
    protected IJSInteropService JsInteropService { get; set; } = null!;

    [Inject]
    private IStringLocalizer<FogResource> Loc { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    [Parameter]
    public EventCallback<HeroProfileIdentifier> OnProfileUpdate { get; set; }

    [Inject]
    private IPersistenceService PersistenceService { get; set; }

    [Parameter]
    public bool ShowBarracksSelector { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        _profile = await HeroProfileUiService.GetHeroProfileAsync(InitProfile);

        if (!OperatingSystem.IsBrowser() || _profile == null)
        {
            return;
        }

        if (OperatingSystem.IsBrowser())
        {
            _showVideoAvatar = (await PersistenceService.GetUiSettingsAsync()).ShowHeroVideoAvatar;
            await JsInteropService.HideLoadingIndicatorAsync();
            StateHasChanged();
        }

        await UpdateProgressionTargetLevels();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_initProfile == InitProfile)
        {
            return;
        }

        _initProfile ??= InitProfile;

        _defaultAnalyticsParameters = new Dictionary<string, object>
        {
            {AnalyticsParams.LOCATION, AnalyticsLocation},
        };

        _profile = await HeroProfileUiService.GetHeroProfileAsync(InitProfile);
        if (_profile != null)
        {
            _defaultAnalyticsParameters.Add(AnalyticsParams.UNIT_ID, _profile.HeroUnitId);
        }

        await UpdateProgressionTargetLevels();

        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender && _profile == null)
        {
            await JsInteropService.ShowLoadingIndicatorAsync();
        }
    }

    private void OnAbilityCalculatorTargetLevelChanged(int targetLevel)
    {
        AnalyticsService.TrackEvent(AnalyticsEvents.PICK_ABILITY_TARGET_LEVEL, _defaultAnalyticsParameters,
            new Dictionary<string, object> {{AnalyticsParams.LEVEL, targetLevel}});
    }

    private Task UpdateProgressionTargetLevels()
    {
        if (_profile == null)
        {
            return Task.CompletedTask;
        }

        var targetLevels = _profile.HeroLevels.SkipWhile(l => l <= _profile.Level).ToList();
        if (targetLevels.Count == 0)
        {
            _progressionTargetLevels = null;
            _progressionTargetLevel = null;
            return Task.CompletedTask;
        }

        _progressionTargetLevels = targetLevels;
        if (_progressionTargetLevel < targetLevels[0])
        {
            _progressionTargetLevel = targetLevels[0];
        }

        return UpdateProgressionCost();
    }

    private async Task UpdateProgressionCost()
    {
        if (_profile == null || _progressionTargetLevel == null)
        {
            return;
        }

        var request = new HeroProgressionCostRequest
        {
            HeroId = _profile.Identifier.HeroId,
            CurrentLevel = _profile.Level,
            TargetLevel = _progressionTargetLevel,
        };

        _progressionCost = await HeroProfileUiService.CalculateHeroProgressionCost(request);
    }

    private Task OnProgressionTargetLevelChanged(HeroLevelSpecs targetLevel)
    {
        _progressionTargetLevel = targetLevel;

        AnalyticsService.TrackProgressionCalculatorEvent(_defaultAnalyticsParameters, _progressionTargetLevel);

        return UpdateProgressionCost();
    }

    private Task OnLevelValueChanged(HeroLevelSpecs level)
    {
        var request = CreateRequest();
        return UpdateProfile(request with {Level = level.Level, AscensionLevel = level.AscensionLevel});
    }

    private Task OnBarracksLevelChanged(BuildingLevelSpecs levelSpecs)
    {
        var request = CreateRequest();
        return UpdateProfile(request with {BarracksLevel = levelSpecs.Level});
    }

    private Task OnAwakeningValueChanged(int level)
    {
        var request = CreateRequest();
        return UpdateProfile(request with {AwakeningLevel = level});
    }

    private Task OnAbilityLevelValueChanged(int level)
    {
        var request = CreateRequest();
        return UpdateProfile(request with {AbilityLevel = level});
    }

    private HeroProfileIdentifier CreateRequest()
    {
        return HeroProfileIdentifierFactory.Create(_profile.Identifier, _profile.Level);
    }

    private async Task UpdateProfile(HeroProfileIdentifier identifier)
    {
        _profile = await HeroProfileUiService.GetHeroProfileAsync(identifier);
        await UpdateProgressionTargetLevels();
        await OnProfileUpdate.InvokeAsync(identifier);
        AnalyticsService.TrackHeroLevelChange(_defaultAnalyticsParameters, identifier);
    }

    private async Task ToggleAvatarSource()
    {
        if (!_showVideoAvatar && (_isVideoAvatarLoadFailed || _profile?.VideoUrl == null))
        {
            return;
        }

        _showVideoAvatar = !_showVideoAvatar;

        AnalyticsService.TrackEvent(AnalyticsEvents.TOGGLE_HERO_AVATAR_SOURCE, _defaultAnalyticsParameters,
            new Dictionary<string, object>
            {
                {
                    AnalyticsParams.AVATAR_SOURCE,
                    _showVideoAvatar
                        ? AnalyticsParams.Values.AvatarSources.VIDEO
                        : AnalyticsParams.Values.AvatarSources.IMAGE
                },
            });
        if (OperatingSystem.IsBrowser())
        {
            var uiSettings = await PersistenceService.GetUiSettingsAsync();
            uiSettings.ShowHeroVideoAvatar = _showVideoAvatar;
            await PersistenceService.SaveUiSettingsAsync(uiSettings);
        }
    }

    private void OnVideoError()
    {
        _isVideoAvatarLoadFailed = true;
        _showVideoAvatar = false;
    }
}
