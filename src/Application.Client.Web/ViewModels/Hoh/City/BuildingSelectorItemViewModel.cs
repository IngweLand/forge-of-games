using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingSelectorItemViewModel
{
    public required BuildingGroup BuildingGroup { get; init; }
    public required string Label { get; init; }
}
