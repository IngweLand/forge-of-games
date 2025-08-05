using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Shared.Helpers.Interfaces;

namespace Ingweland.Fog.WebApp.Apis;

public class HohServices(
    ILogger<HohServices> logger,
    IUnitService unitService,
    ICityService cityService,
    IResearchService researchService,
    ICommandCenterService commandCenterService,
    ICommonService commonService,
    IProtobufSerializer protobufSerializer,
    ICommandCenterProfileRepository commandCenterProfileRepository,
    IInGameStartupDataProcessingService inGameStartupDataProcessingService,
    IInGameStartupDataRepository inGameStartupDataRepository,
    IHohCityRepository hohCityRepository,
    ICampaignService campaignService,
    ITreasureHuntService treasureHuntService,
    IWikipediaService wikipediaService,
    IRelicService relicService)
{
    public ICampaignService CampaignService { get; } = campaignService;
    public ICityService CityService { get; } = cityService;
    public ICommandCenterProfileRepository CommandCenterProfileRepository { get; } = commandCenterProfileRepository;
    public ICommandCenterService CommandCenterService { get; } = commandCenterService;
    public ICommonService CommonService { get; } = commonService;

    public IHohCityRepository HohCityRepository { get; } = hohCityRepository;

    public IInGameStartupDataProcessingService InGameStartupDataProcessingService { get; } =
        inGameStartupDataProcessingService;

    public IInGameStartupDataRepository InGameStartupDataRepository { get; } =
        inGameStartupDataRepository;

    public ILogger<HohServices> Logger { get; } = logger;
    public IProtobufSerializer ProtobufSerializer { get; } = protobufSerializer;
    public IRelicService RelicService { get; } = relicService;
    public IResearchService ResearchService { get; } = researchService;
    public ITreasureHuntService TreasureHuntService { get; } = treasureHuntService;
    public IUnitService UnitService { get; } = unitService;
    public IWikipediaService WikipediaService { get; } = wikipediaService;
}
