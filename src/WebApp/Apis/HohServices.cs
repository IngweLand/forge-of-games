using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using MediatR;

namespace Ingweland.Fog.WebApp.Apis;

public class HohServices(
    ILogger<HohServices> logger,
    ICommandCenterProfileRepository commandCenterProfileRepository,
    IInGameStartupDataProcessingService inGameStartupDataProcessingService,
    IInGameStartupDataRepository inGameStartupDataRepository,
    IHohCityRepository hohCityRepository,
    IWikipediaService wikipediaService,
    IMediator mediator)
{
    public ICommandCenterProfileRepository CommandCenterProfileRepository { get; } = commandCenterProfileRepository;

    public IHohCityRepository HohCityRepository { get; } = hohCityRepository;

    public IInGameStartupDataProcessingService InGameStartupDataProcessingService { get; } =
        inGameStartupDataProcessingService;

    public IInGameStartupDataRepository InGameStartupDataRepository { get; } =
        inGameStartupDataRepository;

    public ILogger<HohServices> Logger { get; } = logger;
    public IMediator Mediator { get; } = mediator;
    public IWikipediaService WikipediaService { get; } = wikipediaService;
}
