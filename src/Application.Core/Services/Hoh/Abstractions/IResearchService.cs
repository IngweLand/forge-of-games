using Ingweland.Fog.Dtos.Hoh.Research;
using Ingweland.Fog.Models.Hoh.Enums;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IResearchService
{
    Task<IReadOnlyCollection<TechnologyDto>> GetTechnologiesAsync([AliasAs("cityId")] CityId cityId);
}
