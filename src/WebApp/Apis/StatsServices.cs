using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using MediatR;

namespace Ingweland.Fog.WebApp.Apis;

public class StatsServices(
    ILogger<StatsServices> logger,
    IMediator mediator)
{
    public ILogger<StatsServices> Logger { get; } = logger;
    public IMediator Mediator { get; } = mediator;
}
