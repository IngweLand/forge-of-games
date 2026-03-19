using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IWonderDtoFactory
{
    WonderDto Create(Wonder wonder);
}
