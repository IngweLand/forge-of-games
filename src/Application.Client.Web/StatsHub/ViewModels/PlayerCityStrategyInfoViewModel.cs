using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class PlayerCityStrategyInfoViewModel
{
    public required CityId CityId { get; init; }
    public required string EventLabel { get; init; }
    public required string IconUrl { get; init; }
    public required int Id { get; init; }
    public required string Label { get; init; }
    public WonderId? Wonder { get; init; }
}
