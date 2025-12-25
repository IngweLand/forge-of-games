using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityStrategyFactory
{
    CityStrategy Create(NewCityRequest newCityRequest, int cityPlannerVersion);
    CityStrategyTimelineLayoutItem CreateTimelineLayoutItem(CityId cityId, WonderId wonderId = WonderId.Undefined);
    CityStrategyTimelineDescriptionItem CreateTimelineDescriptionItem();
    CityStrategyTimelineResearchItem CreateTimelineResearchItem();
    CityStrategyTimelineLayoutItem CreateTimelineLayoutItem(HohCity city);
}
