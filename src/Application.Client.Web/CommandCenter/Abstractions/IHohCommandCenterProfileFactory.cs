using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohCommandCenterProfileFactory
{
    CommandCenterProfile Create(BasicCommandCenterProfile profileDto,
        IReadOnlyDictionary<string, HeroDto> heroes, IReadOnlyCollection<BuildingDto> barracks);

    CommandCenterProfile Create(string profileName);
}
