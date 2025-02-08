using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.Factories.Interfaces;
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
        };
    }

    public HohCity Create(string id, CityId inGameCityId, string ageId, string name,
        IReadOnlyCollection<CityMapEntity> entities, IReadOnlyCollection<HohCitySnapshot> snapshots)
    {
        return new HohCity()
        {
            Id = id,
            InGameCityId = inGameCityId,
            AgeId = ageId,
            Entities = mapper.Map<IReadOnlyCollection<HohCityMapEntity>>(entities),
            Name = name,
            Snapshots = snapshots,
        };
    }
}
