using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICommandCenterService
{
    [Get("/commandCenter/data")]
    Task<CommandCenterDataDto> GetCommandCenterDataAsync();
}
