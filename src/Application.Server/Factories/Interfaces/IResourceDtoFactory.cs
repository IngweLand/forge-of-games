using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IResourceDtoFactory
{
    ResourceDto Create(Resource resource, Age? age);
}
