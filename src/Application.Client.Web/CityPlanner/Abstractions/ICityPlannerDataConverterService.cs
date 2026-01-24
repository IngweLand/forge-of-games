namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerDataConverterService
{
    Task ConvertAsync(string compressedData, string cityName);
}
