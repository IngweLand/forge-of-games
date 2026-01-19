using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using MediatR;

namespace Ingweland.Fog.WebApp.Apis;

public class StatsServices(
    ILogger<StatsServices> logger,
    IBattleService battleService,
    ICityPlannerService cityPlannerService,
    IStatsHubService statsHubService,
    IEquipmentService equipmentService,
    IRelicService relicService,
    IMediator mediator,
    IProtobufResponseFactory protobufResponseFactory)
{
    public IBattleService BattleService { get; } = battleService;
    public ICityPlannerService CityPlannerService { get; } = cityPlannerService;
    public IEquipmentService EquipmentService { get; } = equipmentService;
    public ILogger<StatsServices> Logger { get; } = logger;
    public IMediator Mediator { get; } = mediator;
    public IProtobufResponseFactory ProtobufResponseFactory { get; } = protobufResponseFactory;
    public IRelicService RelicService { get; } = relicService;
    public IStatsHubService StatsHubService { get; } = statsHubService;
}
