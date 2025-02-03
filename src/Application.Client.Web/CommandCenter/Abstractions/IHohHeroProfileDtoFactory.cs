using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IHohHeroProfileDtoFactory
{
    BasicHeroProfile Create(string heroId);
    HeroPlaygroundProfile Create(string id, string heroId, int barracksLevel);
}
