using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class CommandCenterProfileBackup
{
    public required int CommandCenterVersion { get; init; }
    public required BasicCommandCenterProfile Profile { get; init; }
}
