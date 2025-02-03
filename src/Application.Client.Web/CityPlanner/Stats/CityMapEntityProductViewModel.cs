namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapEntityProductViewModel
{
    public required string IconUrl { get; init; }
    public required string OneHourProduction { get; init; }
    public required string OneDayProduction { get; init; }

    public required string ProductId { get; init; }

    public bool IsSelected { get; set; }
}
