using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels;

public class CommunityCityStrategyViewModel
{
    public AgeViewModel? Age { get; set; }
    public required string Author { get; set; }
    public string? CityIconUrl { get; set; }
    public required CityId CityId { get; set; }
    public int? GuideId { get; set; }
    public required string Name { get; set; }
    public required string SharedDataId { get; set; }
    public required string UpdatedAt { get; set; }
}
