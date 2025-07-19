using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
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
    private readonly IBuildingLevelRangesFactory _buildingLevelRangesFactory;
    private readonly IHohCoreDataCache _coreDataCache;
    private readonly Lazy<Task<IReadOnlyCollection<HeroBasicViewModel>>> _heroList;
    private readonly IHohHeroProfileViewModelFactory _heroProfileViewModelFactory;
    private readonly IHeroProgressionCalculators _heroProgressionCalculators;
    private readonly IHeroProfileIdentifierFactory _heroProfileIdentifierFactory;
    private readonly IHohHeroProfileFactory _hohHeroProfileFactory;
    private readonly IMapper _mapper;
    private readonly IPersistenceService _persistenceService;
    private readonly IUnitService _unitService;

    public HeroProfileUiService(
        IPersistenceService persistenceService,
        IHohHeroProfileViewModelFactory heroProfileViewModelFactory,
        IUnitService unitService,
        IHohHeroProfileFactory hohHeroProfileFactory,
        IBuildingLevelRangesFactory buildingLevelRangesFactory,
        IHohCoreDataCache coreDataCache,
        IHeroProgressionCalculators heroProgressionCalculators,
        IHeroProfileIdentifierFactory heroProfileIdentifierFactory,
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
        _mapper = mapper;

        _heroList = new Lazy<Task<IReadOnlyCollection<HeroBasicViewModel>>>(DoGetHeroesAsync);
    }

    public void SaveHeroProfile(HeroProfileIdentifier identifier)
    {
        Task.Run(async () => { await _persistenceService.SaveHeroProfileAsync(identifier); });
    }

    public Task<IReadOnlyCollection<HeroBasicViewModel>> GetHeroes()
    {
        return _heroList.Value;
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

    private async Task<IReadOnlyCollection<HeroBasicViewModel>> DoGetHeroesAsync()
    {
        return _mapper.Map<IReadOnlyCollection<HeroBasicViewModel>>(await _unitService.GetHeroesBasicDataAsync())
            .OrderBy(x => x.Name).ToList();
    }

    private BuildingLevelRange GetBarracksLevels(IReadOnlyCollection<BuildingDto> barracks, UnitType unitType)
    {
        var group = unitType.ToBuildingGroup();
        var ranges = _buildingLevelRangesFactory.Create(barracks);
        return ranges[group];
    }
}
