using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class WonderBasicViewModel
{
    public required WonderId Id { get; init; }
    public required string ImageUrl { get; init; }
    public required string Name { get; init; }
}
