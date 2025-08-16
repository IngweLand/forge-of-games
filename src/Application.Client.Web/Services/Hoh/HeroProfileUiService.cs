using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class HeroProfileUiService : IHeroProfileUiService
{
    private readonly IAssetUrlProvider _assetUrlProvider;
    private readonly IBuildingLevelRangesFactory _buildingLevelRangesFactory;
    private readonly IHohCoreDataCache _coreDataCache;
    private readonly Lazy<Task<IReadOnlyDictionary<string, HeroBasicViewModel>>> _heroList;
    private readonly IHeroProfileIdentifierFactory _heroProfileIdentifierFactory;
    private readonly IHohHeroProfileViewModelFactory _heroProfileViewModelFactory;
    private readonly IHeroProgressionCalculators _heroProgressionCalculators;
    private readonly IHohHeroProfileFactory _hohHeroProfileFactory;
    private readonly IMapper _mapper;
    private readonly IPersistenceService _persistenceService;
    private readonly IUnitService _unitService;

    private IReadOnlyCollection<HeroBasicDto> _heroes = [];

    public HeroProfileUiService(
        IPersistenceService persistenceService,
        IHohHeroProfileViewModelFactory heroProfileViewModelFactory,
        IUnitService unitService,
        IHohHeroProfileFactory hohHeroProfileFactory,
        IBuildingLevelRangesFactory buildingLevelRangesFactory,
        IHohCoreDataCache coreDataCache,
        IHeroProgressionCalculators heroProgressionCalculators,
        IHeroProfileIdentifierFactory heroProfileIdentifierFactory,
        IAssetUrlProvider assetUrlProvider,
        IMapper mapper)
    {
        _persistenceService = persistenceService;
        _heroProfileViewModelFactory = heroProfileViewModelFactory;
        _unitService = unitService;
        _hohHeroProfileFactory = hohHeroProfileFactory;
        _buildingLevelRangesFactory = buildingLevelRangesFactory;
        _coreDataCache = coreDataCache;
        _heroProgressionCalculators = heroProgressionCalculators;
        _heroProfileIdentifierFactory = heroProfileIdentifierFactory;
        _assetUrlProvider = assetUrlProvider;
        _mapper = mapper;

        _heroList = new Lazy<Task<IReadOnlyDictionary<string, HeroBasicViewModel>>>(DoGetHeroesAsync);
    }

    public async Task<IconLabelItemViewModel> CalculateAbilityCostAsync(AbilityCostRequest request)
    {
        var hero = await _coreDataCache.GetHeroAsync(request.HeroId);
        if (hero == null)
        {
            return IconLabelItemViewModel.Blank;
        }

        return new IconLabelItemViewModel
        {
            Label = _heroProgressionCalculators
                .CalculateAbilityCost(hero.Ability, request.CurrentLevel, request.TargetLevel)
                .ToString("N0"),
            IconUrl = _assetUrlProvider.GetHohIconUrl("icon_mastery_points"),
        };
    }

    public void SaveHeroProfile(HeroProfileIdentifier identifier)
    {
        Task.Run(async () => { await _persistenceService.SaveHeroProfileAsync(identifier); });
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes(HeroFilterRequest request)
    {
        var heroVms = await _heroList.Value;
        var query = _heroes.AsEnumerable();

        if (request.Classes.Count > 0)
        {
            query = query.Where(x => request.Classes.Contains(x.ClassId));
        }

        if (request.UnitColors.Count > 0)
        {
            query = query.Where(x => request.UnitColors.Contains(x.UnitColor));
        }

        if (request.UnitTypes.Count > 0)
        {
            query = query.Where(x => request.UnitTypes.Contains(x.UnitType));
        }

        if (request.StarClasses.Count > 0)
        {
            query = query.Where(x => request.StarClasses.Contains(x.StarClass));
        }

        query = query.OrderBy(x => x.Name);
        var result = query.ToList();

        return result.Select(x => heroVms[x.Id]).ToList();
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes(string searchString)
    {
        var heroVms = await _heroList.Value;
        return heroVms
            .Where(kvp => kvp.Value.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            .Select(kvp => kvp.Value).OrderBy(x => x.Name).ToList();
    }

    public async Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes()
    {
        var heroVms = await _heroList.Value;
        return heroVms.Values.ToList();
    }

    public Task<HeroDto?> GetHeroAsync(string heroId)
    {
        return _coreDataCache.GetHeroAsync(heroId);
    }

    public async Task<HeroProfileIdentifier?> GetHeroProfileIdentifierAsync(string heroId)
    {
        var hero = await _coreDataCache.GetHeroAsync(heroId);
        if (hero == null)
        {
            return null;
        }

        if (OperatingSystem.IsBrowser())
        {
            var savedProfile = await _persistenceService.GetHeroProfileAsync(heroId);
            if (savedProfile != null)
            {
                return savedProfile;
            }
        }

        var barracks = await _coreDataCache.GetBarracks(hero.Unit.Type);
        return _heroProfileIdentifierFactory.Create(hero.Id, barracks.OrderBy(x => x.Level).First().Level);
    }

    public async Task<HeroProfileViewModel?> GetHeroProfileAsync(HeroProfileIdentifier identifier)
    {
        var hero = await _coreDataCache.GetHeroAsync(identifier.HeroId);
        if (hero == null)
        {
            return null;
        }

        var barracks = await _coreDataCache.GetBarracks(hero.Unit.Type);
        var profile = _hohHeroProfileFactory.Create(identifier, hero,
            barracks.FirstOrDefault(x => x.Level == identifier.BarracksLevel));
        return _heroProfileViewModelFactory.Create(profile, hero, barracks);
    }

    public async Task<IReadOnlyCollection<IconLabelItemViewModel>> CalculateHeroProgressionCost(
        HeroProgressionCostRequest request)
    {
        var hero = await _coreDataCache.GetHeroAsync(request.HeroId);
        return _mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(
            _heroProgressionCalculators.CalculateProgressionCost(hero!, request.CurrentLevel, request.TargetLevel));
    }

    private async Task<IReadOnlyDictionary<string, HeroBasicViewModel>> DoGetHeroesAsync()
    {
        _heroes = await _unitService.GetHeroesBasicDataAsync();
        return _mapper.Map<IReadOnlyCollection<HeroBasicViewModel>>(_heroes).ToDictionary(x => x.Id);
    }

    private BuildingLevelRange GetBarracksLevels(IReadOnlyCollection<BuildingDto> barracks, UnitType unitType)
    {
        var group = unitType.ToBuildingGroup();
        var ranges = _buildingLevelRangesFactory.Create(barracks);
        return ranges[group];
    }
}
