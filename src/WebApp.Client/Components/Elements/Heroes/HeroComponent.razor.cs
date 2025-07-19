using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Client.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.WebApp.Client.Components.Elements.Heroes;

public partial class HeroComponent : ComponentBase
{
    private HeroProfileIdentifier? _initProfile;
    private bool _isVideoAvatarLoadFailed;
    private HeroProfileViewModel? _profile;
    private IReadOnlyCollection<IconLabelItemViewModel>? _progressionCost;

    private HeroLevelSpecs? _progressionTargetLevel;

    private IList<HeroLevelSpecs>? _progressionTargetLevels;
    private BattleType _selectedBattleType;

    private bool _showVideoAvatar;
    private IReadOnlyDictionary<BattleType, IReadOnlyCollection<UnitBattleViewModel>>? _unitBattles;

    [Inject]
    private IAssetUrlProvider AssetUrlProvider { get; set; }

    [Inject]
    private IBattleSearchRequestFactory BattleSearchRequestFactory { get; set; }

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

    [Parameter]
    public bool ShowBarracksSelector { get; set; } = true;

    [Inject]
    private IStatsHubUiService StatsHubUiService { get; set; }

    [Inject]
    private ITreasureHuntUiService TreasureHuntUiService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _profile = await HeroProfileUiService.GetHeroProfileAsync(InitProfile);

        if (!OperatingSystem.IsBrowser() || _profile == null)
        {
            return;
        }

        if (OperatingSystem.IsBrowser())
        {
            await JsInteropService.HideLoadingIndicatorAsync();
            StateHasChanged();
        }

        await UpdateProgressionTargetLevels();

        var unitBattlesTask = StatsHubUiService.GetUnitBattlesAsync(_profile.HeroUnitId);
        var encounterMapTask = TreasureHuntUiService.GetBattleEncounterToIndexMapAsync();
        await Task.WhenAll(unitBattlesTask, encounterMapTask);
        _unitBattles = (await unitBattlesTask).GroupBy(x => x.BattleType)
            .ToDictionary(x => x.Key, x => (IReadOnlyCollection<UnitBattleViewModel>) x.ToList().AsReadOnly());
        _selectedBattleType = _unitBattles?.FirstOrDefault().Key ?? BattleType.Undefined;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_initProfile == InitProfile)
        {
            return;
        }

        _initProfile ??= InitProfile;

        _profile = await HeroProfileUiService.GetHeroProfileAsync(InitProfile);
        await UpdateProgressionTargetLevels();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender && _profile == null)
        {
            await JsInteropService.ShowLoadingIndicatorAsync();
        }
    }

    private async Task UpdateProgressionTargetLevels()
    {
        var targetLevels = _profile.HeroLevels.SkipWhile(l => l <= _profile.Level).ToList();
        if (targetLevels.Count == 0)
        {
            _progressionTargetLevels = null;
            _progressionTargetLevel = null;
            return;
        }

        _progressionTargetLevels = targetLevels;
        if (_progressionTargetLevel < targetLevels[0])
        {
            _progressionTargetLevel = targetLevels[0];
        }

        await OnProgressionTargetLevelChanged(_progressionTargetLevel!);
    }

    private Task OnLevelValueChanged(HeroLevelSpecs level)
    {
        var request = CreateRequest();
        return UpdateProfile(request with {Level = level.Level, AscensionLevel = level.AscensionLevel});
    }

    private Task OnBarracksLevelChanged(int level)
    {
        var request = CreateRequest();
        return UpdateProfile(request with {BarracksLevel = level});
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
    }

    private async Task OnProgressionTargetLevelChanged(HeroLevelSpecs targetLevel)
    {
        _progressionTargetLevel = targetLevel;
        var request = new HeroProgressionCostRequest
        {
            HeroId = _profile.Identifier.HeroId,
            CurrentLevel = _profile.Level,
            TargetLevel = targetLevel,
        };

        _progressionCost = await HeroProfileUiService.CalculateHeroProgressionCost(request);
    }

    private void OnContributionPromptClick()
    {
        NavigationManager.NavigateTo(FogUrlBuilder.PageRoutes.HELP_BATTLE_LOG_PATH);
    }

    private async Task OpenBattle(UnitBattleViewModel unitBattle)
    {
        var query = BattleSearchRequestFactory.CreateQueryParams(unitBattle.BattleDefinitionId, unitBattle.Difficulty,
            unitBattle.BattleType, [unitBattle.UnitId, unitBattle.UnitId],
            await TreasureHuntUiService.GetBattleEncounterToIndexMapAsync());

        NavigationManager.NavigateTo(
            NavigationManager.GetUriWithQueryParameters(FogUrlBuilder.PageRoutes.BATTLE_LOG_PATH, query), false);
    }

    private void ToggleAvatarSource()
    {
        if (!_showVideoAvatar && (_isVideoAvatarLoadFailed || _profile?.VideoUrl == null))
        {
            return;
        }

        _showVideoAvatar = !_showVideoAvatar;
    }

    private void OnVideoError()
    {
        _isVideoAvatarLoadFailed = true;
        _showVideoAvatar = false;
    }
}
