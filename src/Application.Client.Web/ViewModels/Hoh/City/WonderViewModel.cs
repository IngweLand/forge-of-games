using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class WonderViewModel
{
    public required string CityName { get; init; }
    public required WonderDto Data { get; init; }
    public required WonderId Id { get; init; }
    public required IReadOnlyList<WonderLevelViewModel> Levels { get; init; } = new List<WonderLevelViewModel>();
    public required string Name { get; init; }
}
