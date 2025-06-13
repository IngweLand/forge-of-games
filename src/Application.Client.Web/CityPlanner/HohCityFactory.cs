using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Constants;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class HohCityFactory(IMapper mapper) : IHohCityFactory
{
    public HohCity CreateNewCapital()
    {
        return CreateNewCapital($"{CityId.Capital} - {DateTime.Now:g}");
    }

    public HohCity CreateNewCapital(string cityName)
    {
        return new HohCity()
        {
            Id = Guid.NewGuid().ToString(),
            InGameCityId = CityId.Capital,
            AgeId = AgeIds.BRONZE_AGE,
            Entities = new List<HohCityMapEntity>()
            {
                new()
                {
                    Id = 0,
                    CityEntityId = "Building_StoneAge_City_CityHall_1",
                    Level = 2,
                    X = 46,
                    Y = -51,
                },
            },
            Name = cityName,
            UnlockedExpansions = InitCityConfigs.Expansions[CityId.Capital],
        };
    }

    public HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IReadOnlyCollection<CityMapEntity> entities, IReadOnlyCollection<HohCitySnapshot> snapshots,
        IEnumerable<string> expansions, WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0)
    {
        return new HohCity()
        {
            Id = id,
            InGameCityId = inGameCityId,
            AgeId = ageId,
            Entities = mapper.Map<IReadOnlyCollection<HohCityMapEntity>>(entities),
            Name = name,
            Snapshots = snapshots,
            UnlockedExpansions = expansions.ToHashSet(),
            WonderId = cityWonderId,
            WonderLevel = cityWonderLevel,
        };
    }

    private static class InitCityConfigs
    {
        public static readonly Dictionary<CityId, HashSet<string>> Expansions =
            new()
            {
                {
                    CityId.Capital,
                    [
                        "Expansion_Capital_1", "Expansion_Capital_2", "Expansion_Capital_3", "Expansion_Capital_4",
                        "Expansion_Capital_5", "Expansion_Capital_6", "Expansion_Capital_7", "Expansion_Capital_8",
                        "Expansion_Capital_9"
                    ]
                },
                {
                    CityId.China,
                    [
                        "Expansion_China_1", "Expansion_China_2", "Expansion_China_3", "Expansion_China_4",
                        "Expansion_China_5", "Expansion_China_6", "Expansion_China_7"
                    ]
                },
                {
                    CityId.Vikings,
                    [
                        "Expansion_Vikings_1", "Expansion_Vikings_2", "Expansion_Vikings_3", "Expansion_Vikings_4",
                        "Expansion_Vikings_5", "Expansion_Vikings_6", "Expansion_Vikings_7", "Expansion_Vikings_8",
                        "Expansion_Vikings_9", "Expansion_Vikings_10"
                    ]
                },
                {
                    CityId.Egypt,
                    [
                        "Expansion_Egypt_1", "Expansion_Egypt_2", "Expansion_Egypt_3", "Expansion_Egypt_4",
                        "Expansion_Egypt_5", "Expansion_Egypt_6", "Expansion_Egypt_7", "Expansion_Egypt_8",
                        "Expansion_Egypt_9"
                    ]
                },
                {
                    CityId.Mayas_SayilPalace,
                    [
                        "Expansion_Mayas_SayilPalace_1", "Expansion_Mayas_SayilPalace_2",
                        "Expansion_Mayas_SayilPalace_3", "Expansion_Mayas_SayilPalace_4",
                        "Expansion_Mayas_SayilPalace_5"
                    ]
                },
                {
                    CityId.Mayas_ChichenItza,
                    [
                        "Expansion_Mayas_ChichenItza_1", "Expansion_Mayas_ChichenItza_2",
                        "Expansion_Mayas_ChichenItza_3", "Expansion_Mayas_ChichenItza_4",
                        "Expansion_Mayas_ChichenItza_5"
                    ]
                },
                {
                    CityId.Mayas_Tikal,
                    [
                        "Expansion_Mayas_Tikal_1", "Expansion_Mayas_Tikal_2", "Expansion_Mayas_Tikal_3",
                        "Expansion_Mayas_Tikal_4", "Expansion_Mayas_Tikal_5"
                    ]
                }
            };
    }
}
