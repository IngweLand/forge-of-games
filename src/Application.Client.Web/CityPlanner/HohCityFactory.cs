using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class HohCityFactory(IMapper mapper) : IHohCityFactory
{
    public HohCity CreateNewCapital(int cityPlannerVersion)
    {
        return Create(new NewCityRequest {Name = $"{CityId.Capital}", CityId = CityId.Capital},
            cityPlannerVersion);
    }

    public HohCity Create(NewCityRequest newCityRequest, int cityPlannerVersion)
    {
        var searchId = newCityRequest.CityId.ToString();
        if (newCityRequest.CityId is CityId.Mayas_ChichenItza or CityId.Mayas_SayilPalace or CityId.Mayas_Tikal)
        {
            searchId = newCityRequest.WonderId.ToString();
        }
        else if (newCityRequest.CityId is CityId.Arabia_CityOfBrass or CityId.Arabia_NoriasOfHama
                 or CityId.Arabia_Petra)
        {
            searchId = newCityRequest.WonderId.ToString();
        }

        return new HohCity
        {
            Id = Guid.NewGuid().ToString(),
            InGameCityId = newCityRequest.CityId,
            AgeId = newCityRequest.CityId.ToDefaultAge(),
            Entities = InitCityConfigs.GetMapEntities(searchId),
            Name = newCityRequest.Name,
            UnlockedExpansions = InitCityConfigs.GetExpansions(newCityRequest.CityId),
            WonderId = newCityRequest.WonderId,
            UpdatedAt = DateTime.Now.ToLocalTime(),
            CityPlannerVersion = cityPlannerVersion,
        };
    }

    public HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IEnumerable<CityMapEntity> entities, IEnumerable<CityMapEntity> inventoryBuildings,
        IReadOnlyCollection<HohCitySnapshot> snapshots, IEnumerable<string> expansions, int cityPlannerVersion,
        WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0)
    {
        return new HohCity
        {
            Id = id,
            InGameCityId = inGameCityId,
            AgeId = ageId,
            Entities = mapper.Map<IReadOnlyCollection<HohCityMapEntity>>(entities),
            InventoryBuildings = mapper.Map<IReadOnlyCollection<HohCityMapEntity>>(inventoryBuildings),
            Name = name,
            Snapshots = snapshots,
            UnlockedExpansions = expansions.ToHashSet(),
            WonderId = cityWonderId,
            WonderLevel = cityWonderLevel,
            UpdatedAt = DateTime.Now.ToLocalTime(),
            CityPlannerVersion = cityPlannerVersion,
        };
    }

    public HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IReadOnlyCollection<HohCityMapEntity> entities, HashSet<string> expansions, int cityPlannerVersion,
        WonderId cityWonderId = WonderId.Undefined, int cityWonderLevel = 0)
    {
        return new HohCity
        {
            Id = id,
            InGameCityId = inGameCityId,
            AgeId = ageId,
            Entities = entities,
            InventoryBuildings = [],
            Name = name,
            Snapshots = [],
            UnlockedExpansions = expansions,
            WonderId = cityWonderId,
            WonderLevel = cityWonderLevel,
            UpdatedAt = DateTime.Now.ToLocalTime(),
            CityPlannerVersion = cityPlannerVersion,
        };
    }

    private static class InitCityConfigs
    {
        public static List<HohCityMapEntity> GetMapEntities(string key)
        {
            return key switch
            {
                nameof(CityId.Capital) =>
                [
                    new HohCityMapEntity
                    {
                        Id = 0,
                        CityEntityId = "Building_StoneAge_City_CityHall_1",
                        Level = 2,
                        X = 46,
                        Y = -51,
                    },
                ],
                nameof(CityId.Vikings) =>
                [
                    new HohCityMapEntity
                    {
                        Id = 0,
                        CityEntityId = "Building_Vikings_City_CityHall_1",
                        Level = 1,
                        IsRotated = true,
                        X = 30,
                        Y = -48,
                    },
                ],
                nameof(CityId.Egypt) =>
                [
                    new HohCityMapEntity
                    {
                        Id = 0,
                        CityEntityId = "Building_Egypt_City_CityHall_1",
                        Level = 1,
                        X = 46,
                        Y = -51,
                    },
                ],
                nameof(WonderId.Mayas_ChichenItza) =>
                [
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Premium_1",
                        Id = 0,
                        Level = 1,
                        X = 27,
                        Y = -39,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 1,
                        Level = 1,
                        X = 44,
                        Y = -26,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 2,
                        IsRotated = true,
                        Level = 1,
                        X = 43,
                        Y = -35,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 3,
                        IsRotated = true,
                        Level = 1,
                        X = 23,
                        Y = -47,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 4,
                        IsRotated = true,
                        Level = 1,
                        X = 24,
                        Y = -30,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_City_CityHall_1",
                        Id = 5,
                        Level = 1,
                        SelectedProductId = "Production1_Building_Mayas_City_CityHall_1",
                        X = 34,
                        Y = -39,
                    },
                ],
                nameof(WonderId.Mayas_Tikal) =>
                [
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Premium_1",
                        Id = 0,
                        Level = 1,
                        X = 23,
                        Y = -38,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 1,
                        Level = 1,
                        X = 40,
                        Y = -30,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 2,
                        Level = 1,
                        X = 24,
                        Y = -27,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 3,
                        IsRotated = true,
                        Level = 1,
                        X = 44,
                        Y = -38,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 4,
                        IsRotated = true,
                        Level = 1,
                        X = 28,
                        Y = -46,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_City_CityHall_1",
                        Id = 5,
                        Level = 1,
                        SelectedProductId = "Production1_Building_Mayas_City_CityHall_1",
                        X = 34,
                        Y = -39,
                    },
                ],
                nameof(WonderId.Mayas_SayilPalace) =>
                [
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Premium_1",
                        Id = 0,
                        Level = 1,
                        X = 36,
                        Y = -26,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 1,
                        Level = 1,
                        X = 36,
                        Y = -47,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 2,
                        IsRotated = true,
                        Level = 1,
                        X = 44,
                        Y = -47,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 3,
                        IsRotated = true,
                        Level = 1,
                        X = 27,
                        Y = -30,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_RitualSite_Average_1",
                        Id = 4,
                        IsRotated = true,
                        Level = 1,
                        X = 44,
                        Y = -31,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_Mayas_City_CityHall_1",
                        Id = 5,
                        Level = 1,
                        SelectedProductId = "Production1_Building_Mayas_City_CityHall_1",
                        X = 34,
                        Y = -39,
                    },
                ],
                nameof(CityId.China) =>
                [
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_City_CityHall_1",
                        Id = 9,
                        IsRotated = true,
                        Level = 1,
                        SelectedProductId = "Production1_Building_China_City_CityHall_1",
                        X = 30,
                        Y = -51,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_KaolinQuarry_1",
                        Id = 17,
                        IsRotated = true,
                        Level = 1,
                        X = 35,
                        Y = -39,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_MothGlade_1",
                        Id = 18,
                        Level = 1,
                        X = 11,
                        Y = -51,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_MothGlade_1",
                        Id = 19,
                        Level = 1,
                        X = 11,
                        Y = -27,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_KaolinQuarry_1",
                        Id = 20,
                        IsRotated = true,
                        Level = 1,
                        X = 31,
                        Y = -31,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_KaolinQuarry_1",
                        Id = 21,
                        IsRotated = true,
                        Level = 1,
                        X = 43,
                        Y = -31,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_KaolinQuarry_1",
                        Id = 22,
                        IsRotated = true,
                        Level = 1,
                        X = 43,
                        Y = -43,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_MothGlade_1",
                        Id = 23,
                        Level = 1,
                        X = 23,
                        Y = -23,
                        IsLocked = true,
                    },
                    new HohCityMapEntity
                    {
                        CityEntityId = "Building_China_ExtractionPoint_MothGlade_1",
                        Id = 2,
                        Level = 1,
                        X = 19,
                        Y = -43,
                        IsLocked = true,
                    },
                ],
                nameof(CityId.Arabia_CityOfBrass) or
                    nameof(CityId.Arabia_NoriasOfHama) or
                    nameof(CityId.Arabia_Petra) =>
                    [
                        new HohCityMapEntity
                        {
                            Id = 0,
                            CityEntityId = "Building_Arabia_City_CityHall_1",
                            Level = 1,
                            X = 45,
                            Y = -51,
                        },
                        new HohCityMapEntity
                        {
                            Id = 1,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 15,
                            Y = -45,
                            IsRotated = true,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 2,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 21,
                            Y = -48,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 3,
                            CityEntityId = "Building_Arabia_Irrigation_Noria_1",
                            Level = 1,
                            X = 27,
                            Y = -39,
                            IsRotated = true,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 4,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 39,
                            Y = -45,
                            IsRotated = true,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 5,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 36,
                            Y = -12,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 6,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 21,
                            Y = -12,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 7,
                            CityEntityId = "Building_Arabia_Irrigation_Noria_1",
                            Level = 1,
                            X = 36,
                            Y = -27,
                            IsRotated = true,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 8,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 24,
                            Y = -21,
                            IsRotated = true,
                            IsLocked = true,
                        },
                        new HohCityMapEntity
                        {
                            Id = 9,
                            CityEntityId = "Building_Arabia_Irrigation_Oasis_1",
                            Level = 1,
                            X = 9,
                            Y = -36,
                            IsRotated = true,
                            IsLocked = true,
                        },
                    ],
                _ => [],
            };
        }

        public static HashSet<string> GetExpansions(CityId key)
        {
            return key switch
            {
                CityId.Capital =>
                [
                    "Expansion_Capital_1", "Expansion_Capital_2", "Expansion_Capital_3", "Expansion_Capital_4",
                    "Expansion_Capital_5", "Expansion_Capital_6", "Expansion_Capital_7", "Expansion_Capital_8",
                    "Expansion_Capital_9",
                ],
                CityId.China =>
                [
                    "Expansion_China_1", "Expansion_China_2", "Expansion_China_3", "Expansion_China_4",
                    "Expansion_China_5", "Expansion_China_6", "Expansion_China_7",
                ],
                CityId.Vikings =>
                [
                    "Expansion_Vikings_1", "Expansion_Vikings_2", "Expansion_Vikings_3", "Expansion_Vikings_4",
                    "Expansion_Vikings_5", "Expansion_Vikings_6", "Expansion_Vikings_7", "Expansion_Vikings_8",
                    "Expansion_Vikings_9", "Expansion_Vikings_10",
                ],
                CityId.Egypt =>
                [
                    "Expansion_Egypt_1", "Expansion_Egypt_2", "Expansion_Egypt_3", "Expansion_Egypt_4",
                    "Expansion_Egypt_5", "Expansion_Egypt_6", "Expansion_Egypt_7", "Expansion_Egypt_8",
                    "Expansion_Egypt_9",
                ],
                CityId.Mayas_SayilPalace =>
                [
                    "Expansion_Mayas_SayilPalace_1", "Expansion_Mayas_SayilPalace_2",
                    "Expansion_Mayas_SayilPalace_3", "Expansion_Mayas_SayilPalace_4",
                    "Expansion_Mayas_SayilPalace_5",
                ],
                CityId.Mayas_ChichenItza =>
                [
                    "Expansion_Mayas_ChichenItza_1", "Expansion_Mayas_ChichenItza_2",
                    "Expansion_Mayas_ChichenItza_3", "Expansion_Mayas_ChichenItza_4",
                    "Expansion_Mayas_ChichenItza_5",
                ],
                CityId.Mayas_Tikal =>
                [
                    "Expansion_Mayas_Tikal_1", "Expansion_Mayas_Tikal_2", "Expansion_Mayas_Tikal_3",
                    "Expansion_Mayas_Tikal_4", "Expansion_Mayas_Tikal_5",
                ],
                CityId.Arabia_CityOfBrass =>
                [
                    "Expansion_Arabia_CityOfBrass_1", "Expansion_Arabia_CityOfBrass_2",
                    "Expansion_Arabia_CityOfBrass_3", "Expansion_Arabia_CityOfBrass_4",
                    "Expansion_Arabia_CityOfBrass_5", "Expansion_Arabia_CityOfBrass_6",
                    "Expansion_Arabia_CityOfBrass_7", "Expansion_Arabia_CityOfBrass_8",
                    "Expansion_Arabia_CityOfBrass_9", "Expansion_Arabia_CityOfBrass_10",
                    "Expansion_Arabia_CityOfBrass_11", "Expansion_Arabia_CityOfBrass_12",
                    "Expansion_Arabia_CityOfBrass_13", "Expansion_Arabia_CityOfBrass_14",
                    "Expansion_Arabia_CityOfBrass_15", "Expansion_Arabia_CityOfBrass_16",
                ],
                CityId.Arabia_NoriasOfHama =>
                [
                    "Expansion_Arabia_NoriasOfHama_1", "Expansion_Arabia_NoriasOfHama_2",
                    "Expansion_Arabia_NoriasOfHama_3", "Expansion_Arabia_NoriasOfHama_4",
                    "Expansion_Arabia_NoriasOfHama_5", "Expansion_Arabia_NoriasOfHama_6",
                    "Expansion_Arabia_NoriasOfHama_7", "Expansion_Arabia_NoriasOfHama_8",
                    "Expansion_Arabia_NoriasOfHama_9", "Expansion_Arabia_NoriasOfHama_10",
                    "Expansion_Arabia_NoriasOfHama_11", "Expansion_Arabia_NoriasOfHama_12",
                    "Expansion_Arabia_NoriasOfHama_13", "Expansion_Arabia_NoriasOfHama_14",
                    "Expansion_Arabia_NoriasOfHama_15", "Expansion_Arabia_NoriasOfHama_16",
                ],
                CityId.Arabia_Petra =>
                [
                    "Expansion_Arabia_Petra_1", "Expansion_Arabia_Petra_2", "Expansion_Arabia_Petra_3",
                    "Expansion_Arabia_Petra_4", "Expansion_Arabia_Petra_5", "Expansion_Arabia_Petra_6",
                    "Expansion_Arabia_Petra_7", "Expansion_Arabia_Petra_8", "Expansion_Arabia_Petra_9",
                    "Expansion_Arabia_Petra_10", "Expansion_Arabia_Petra_11", "Expansion_Arabia_Petra_12",
                    "Expansion_Arabia_Petra_13", "Expansion_Arabia_Petra_14", "Expansion_Arabia_Petra_15",
                    "Expansion_Arabia_Petra_16",
                ],
                _ => [],
            };
        }
    }
}
