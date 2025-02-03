using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class WonderGroupViewModel
{
    public CityId CityId { get; set; }
    public required string CityName { get; init; }
    public required IReadOnlyCollection<WonderBasicViewModel> Wonders { get; init; } = new List<WonderBasicViewModel>();
}
