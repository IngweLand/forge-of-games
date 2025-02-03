using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class ContinentBasicViewModel
{
    public required ContinentId Id { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyCollection<RegionBasicViewModel> Regions { get; init; }
}
