using Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder;

public class CityStrategyFactory(IHohCityFactory cityFactory, IStringLocalizer<FogResource> loc) : ICityStrategyFactory
{
    public CityStrategy Create(NewCityRequest newCityRequest, int cityPlannerVersion)
    {
        var layoutItem = CreateLayoutTimelineItem(newCityRequest);
        var strategy = new CityStrategy
        {
            Id = Guid.NewGuid().ToString(),
            InGameCityId = newCityRequest.CityId,
            Name = newCityRequest.Name,
            WonderId = newCityRequest.WonderId,
            UpdatedAt = DateTime.Now,
            CityPlannerVersion = cityPlannerVersion,
            AgeId = layoutItem.AgeId,
        };
        strategy.Timeline.Add(CreateIntroTimelineItem(newCityRequest.CityId, newCityRequest.WonderId));
        strategy.Timeline.Add(CreateDescriptionTimelineItem());
        strategy.Timeline.Add(CreateResearchTimelineItem());
        strategy.Timeline.Add(layoutItem);

        return strategy;
    }

    public CityStrategyDescriptionTimelineItem CreateDescriptionTimelineItem()
    {
        return new CityStrategyDescriptionTimelineItem
        {
            Title = loc[FogResource.CityStrategy_TimelineDescriptionItem_DefaultTitle],
            Description = loc[FogResource.CityStrategy_TimelineDescriptionItem_DefaultDescription],
        };
    }

    public CityStrategyResearchTimelineItem CreateResearchTimelineItem()
    {
        return new CityStrategyResearchTimelineItem
        {
            Title = loc[FogResource.CityStrategy_TimelineResearchItem_DefaultTitle],
        };
    }

    public CityStrategyLayoutTimelineItem CreateLayoutTimelineItem(CityId cityId,
        WonderId wonderId = WonderId.Undefined)
    {
        var request = new NewCityRequest
        {
            CityId = cityId,
            WonderId = wonderId,
            Name = string.Empty,
        };
        return CreateLayoutTimelineItem(request);
    }

    public CityStrategyLayoutTimelineItem CreateLayoutTimelineItem(HohCity city)
    {
        return new CityStrategyLayoutTimelineItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = loc[FogResource.CityStrategy_TimelineLayoutItem_DefaultTitle],
            AgeId = city.AgeId,
            UnlockedExpansions = city.UnlockedExpansions,
            WonderLevel = city.WonderLevel,
            Entities = city.Entities,
        };
    }

    public CityStrategyIntroTimelineItem CreateIntroTimelineItem(CityId cityId, WonderId wonderId)
    {
        return new CityStrategyIntroTimelineItem
        {
            Title = wonderId.ToString(),
            Description = loc[FogResource.CityStrategy_TimelineDescriptionItem_DefaultDescription],
            CityId = cityId,
            WonderId = wonderId,
        };
    }

    private CityStrategyLayoutTimelineItem CreateLayoutTimelineItem(NewCityRequest request)
    {
        var city = cityFactory.Create(request, 0);
        return CreateLayoutTimelineItem(city);
    }
}
