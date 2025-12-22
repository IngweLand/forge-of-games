using MediatR;

namespace Ingweland.Fog.WebApp.Apis;

public class FogServices(IMediator mediator)
{
    public IMediator Mediator { get; } = mediator;
}
