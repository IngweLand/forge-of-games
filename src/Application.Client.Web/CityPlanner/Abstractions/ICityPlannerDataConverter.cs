using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerDataConverter
{
    Task<Result<HohCity>> ConvertAsync(string compressedData, string cityName, IProgress<string>? progress = null);
    Result<string> ParseUrl(string url);
}
