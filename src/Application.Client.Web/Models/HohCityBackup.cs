using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class HohCityBackup
{
    public required int CityPlannerVersion { get; init; }
    public required HohCity City { get; init; }
}