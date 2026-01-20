using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Abstractions;
using Ingweland.Fog.Functions.Functions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Services;

public interface IEventCityStrategyFactory
{
    Task RunAsync();
}

public class EventCityStrategyFactory(
    IFogDbContext context,
    IPlayerCityService playerCityService,
    IProtobufSerializer protobufSerializer,
    ILogger<EventCityStrategyFactory> logger) : IEventCityStrategyFactory
{
    public async Task RunAsync()
    {
         var snapshotGroups = await context.EventCitySnapshots.AsNoTracking().ToListAsync();
        var comparer = new HohCitySpecificComparer();
        foreach (var sg in snapshotGroups.GroupBy(x => x.PlayerId))
        {
            var first = sg.First();
            var inGameEvent = await context.InGameEvents.FindAsync(first.InGameEventId);
            if (inGameEvent == null)
            {
                logger.LogWarning("Could not find event with id {eventId}", first.InGameEventId);
                continue;
            }
            var hasPremiumExpansions = false;
            CityStrategy? strategy = null;
            HohCity? lastCity = null;
            foreach (var snapshot in sg.OrderBy(x => x.CollectedAt))
            {
                var cityResult = await playerCityService.GetEventCityAsync(snapshot.Id);
                if (!cityResult.IsSuccess)
                {
                    cityResult.LogIfFailed<EventCityStrategyFactoryTrigger>($"Could not create event city for snapshot {
                        snapshot.Id}");
                    continue;
                }

                if (strategy == null)
                {
                    strategy = Create(cityResult.Value.InGameCityId, cityResult.Value.WonderId, cityResult.Value.AgeId,
                        cityResult.Value.Name);
                    strategy.Timeline.Add(CreateTimelineLayoutItem(cityResult.Value,
                        CreateTimelineLayoutItemTitle(snapshot.CollectedAt, inGameEvent!.StartAt,
                            cityResult.Value.WonderLevel)));
                }
                else if (!comparer.Equals(cityResult.Value, lastCity))
                {
                    strategy.Timeline.Add(CreateTimelineLayoutItem(cityResult.Value,
                        CreateTimelineLayoutItemTitle(snapshot.CollectedAt, inGameEvent!.StartAt,
                            cityResult.Value.WonderLevel)));
                }

                lastCity = cityResult.Value;
                hasPremiumExpansions = hasPremiumExpansions || cityResult.Value.PremiumExpansionCount > 0;
            }

            context.EventCityStrategies.Add(new EventCityStrategy
            {
                Data = new EventCityStrategyDataEntity
                {
                    Data = protobufSerializer.SerializeToBytes(strategy!),
                },
                CityId = first.CityId,
                WonderId = first.WonderId,
                HasPremiumBuildings = first.HasPremiumBuildings,
                HasPremiumExpansion = hasPremiumExpansions,
                InGameEventId = inGameEvent.Id,
                PlayerId = first.PlayerId,
            });
            await context.SaveChangesAsync();
            logger.LogInformation("Created event city strategy for player {playerId}", first.PlayerId);
        }
    }
    
     private static string CreateTimelineLayoutItemTitle(DateTime snapshotTime, DateTime eventStartTime, int wonderLevel)
    {
        return $"{(snapshotTime - eventStartTime).ToShortReadableString()}, lvl. {wonderLevel}";
    }

    private CityStrategy Create(CityId cityId, WonderId wonderId, string ageId, string cityName)
    {
        var strategy = new CityStrategy
        {
            Id = Guid.NewGuid().ToString(),
            InGameCityId = cityId,
            Name = cityName,
            WonderId = wonderId,
            UpdatedAt = DateTime.Now,
            CityPlannerVersion = FogConstants.CITY_PLANNER_VERSION,
            AgeId = ageId,
        };

        return strategy;
    }

    private CityStrategyTimelineLayoutItem CreateTimelineLayoutItem(HohCity city, string title)
    {
        return new CityStrategyTimelineLayoutItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            AgeId = city.AgeId,
            UnlockedExpansions = city.UnlockedExpansions,
            WonderLevel = city.WonderLevel,
            Entities = city.Entities,
        };
    }

    private sealed class HohCitySpecificComparer : IEqualityComparer<HohCity>
    {
        public bool Equals(HohCity? x, HohCity? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (x.WonderId != y.WonderId ||
                x.WonderLevel != y.WonderLevel ||
                x.PremiumExpansionCount != y.PremiumExpansionCount)
            {
                return false;
            }

            if (x.Entities.Count != y.Entities.Count)
            {
                return false;
            }

            // Sort both by a stable set of properties (X, Y, then Type) 
            // to ensure we are comparing the exact same logical "slots"
            var sortedX = x.Entities
                .OrderBy(e => e.X)
                .ThenBy(e => e.Y)
                .ThenBy(e => e.CityEntityId);

            var sortedY = y.Entities
                .OrderBy(e => e.X)
                .ThenBy(e => e.Y)
                .ThenBy(e => e.CityEntityId);

            return sortedX.SequenceEqual(sortedY, new MapEntityEqualityComparer());
        }

        public int GetHashCode(HohCity obj)
        {
            var hash = new HashCode();
            hash.Add(obj.WonderId);
            hash.Add(obj.WonderLevel);
            hash.Add(obj.PremiumExpansionCount);
            hash.Add(obj.Entities.Count);
            return hash.ToHashCode();
        }

        private sealed class MapEntityEqualityComparer : IEqualityComparer<HohCityMapEntity>
        {
            public bool Equals(HohCityMapEntity? a, HohCityMapEntity? b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                if (a is null || b is null)
                {
                    return false;
                }

                return a.CityEntityId == b.CityEntityId &&
                    a.CustomizationId == b.CustomizationId &&
                    a.IsLocked == b.IsLocked &&
                    a.IsRotated == b.IsRotated &&
                    a.Level == b.Level &&
                    a.SelectedProductId == b.SelectedProductId &&
                    a.X == b.X &&
                    a.Y == b.Y &&
                    a.IsUpgrading == b.IsUpgrading;
                //ignoring Id
            }

            public int GetHashCode(HohCityMapEntity obj)
            {
                return 0;
                // Not used by SequenceEqual for logic
            }
        }
    }
}
