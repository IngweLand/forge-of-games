using Ingweland.Fog.Dtos.Hoh.CommandCenter;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICommandCenterService
{
    Task<CommandCenterDataDto> GetCommandCenterDataAsync();
}
