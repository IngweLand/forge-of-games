using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels;

public class CommunityCityGuideViewModel
{
    public AgeViewModel? Age { get; init; }
    public required string Author { get; init; }
    public string? CityIconUrl { get; init; }
    public required CityId CityId { get; init; }
    public string Content { get; init; } = string.Empty;
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string UpdatedAt { get; init; }
}
