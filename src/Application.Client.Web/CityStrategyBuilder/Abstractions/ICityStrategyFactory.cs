using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityStrategyFactory
{
    CityStrategy Create(NewCityRequest newCityRequest, int cityPlannerVersion);
    CityStrategyLayoutTimelineItem CreateLayoutTimelineItem(CityId cityId, WonderId wonderId = WonderId.Undefined);
    CityStrategyDescriptionTimelineItem CreateDescriptionTimelineItem();
    CityStrategyIntroTimelineItem CreateIntroTimelineItem(CityId cityId, WonderId wonderId);
    CityStrategyResearchTimelineItem CreateResearchTimelineItem();
    CityStrategyLayoutTimelineItem CreateLayoutTimelineItem(HohCity city);
}
