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
        var layoutItem = CreateTimelineLayoutItem(newCityRequest);
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
        strategy.Timeline.Add(CreateTimelineDescriptionItem());
        strategy.Timeline.Add(CreateTimelineResearchItem());
        strategy.Timeline.Add(layoutItem);

        return strategy;
    }

    public CityStrategyTimelineDescriptionItem CreateTimelineDescriptionItem()
    {
        return new CityStrategyTimelineDescriptionItem
        {
            Title = loc[FogResource.CityStrategy_TimelineDescriptionItem_DefaultTitle],
            Description = loc[FogResource.CityStrategy_TimelineDescriptionItem_DefaultDescription],
        };
    }

    public CityStrategyTimelineResearchItem CreateTimelineResearchItem()
    {
        return new CityStrategyTimelineResearchItem
        {
            Title = loc[FogResource.CityStrategy_TimelineResearchItem_DefaultTitle],
        };
    }

    public CityStrategyTimelineLayoutItem CreateTimelineLayoutItem(CityId cityId,
        WonderId wonderId = WonderId.Undefined)
    {
        var request = new NewCityRequest
        {
            CityId = cityId,
            WonderId = wonderId,
            Name = string.Empty,
        };
        return CreateTimelineLayoutItem(request);
    }

    public CityStrategyTimelineLayoutItem CreateTimelineLayoutItem(HohCity city)
    {
        return new CityStrategyTimelineLayoutItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = loc[FogResource.CityStrategy_TimelineLayoutItem_DefaultTitle],
            AgeId = city.AgeId,
            UnlockedExpansions = city.UnlockedExpansions,
            WonderLevel = city.WonderLevel,
            Entities = city.Entities,
        };
    }

    private CityStrategyTimelineLayoutItem CreateTimelineLayoutItem(NewCityRequest request)
    {
        var city = cityFactory.Create(request, 0);
        return CreateTimelineLayoutItem(city);
    }
}
