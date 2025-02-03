using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh;
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
    IInGameStartupDataRepository inGameStartupDataRepository)
{
    public ICityService CityService { get; } = cityService;
    public ICommandCenterProfileRepository CommandCenterProfileRepository { get; } = commandCenterProfileRepository;
    public ICommandCenterService CommandCenterService { get; } = commandCenterService;
    public ICommonService CommonService { get; } = commonService;

    public IInGameStartupDataProcessingService InGameStartupDataProcessingService { get; } =
        inGameStartupDataProcessingService;

    public IInGameStartupDataRepository InGameStartupDataRepository { get; } =
        inGameStartupDataRepository;

    public ILogger<HohServices> Logger { get; } = logger;
    public IProtobufSerializer ProtobufSerializer { get; } = protobufSerializer;
    public IResearchService ResearchService { get; } = researchService;
    public IUnitService UnitService { get; } = unitService;
}
