using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using MediatR;

namespace Ingweland.Fog.WebApp.Apis;

public class StatsServices(
    ILogger<StatsServices> logger,
    IBattleService battleService,
    IMediator mediator)
{
    public IBattleService BattleService { get; } = battleService;
    public ILogger<StatsServices> Logger { get; } = logger;
    public IMediator Mediator { get; } = mediator;
}
