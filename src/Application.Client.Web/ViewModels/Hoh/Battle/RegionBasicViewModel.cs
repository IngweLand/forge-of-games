using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class RegionBasicViewModel
{
    public required int DisplayIndex { get; init; }
    public required RegionId Id { get; init; }
    public required string Name { get; init; }
}
