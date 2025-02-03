using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface IBarracksViewModelFactory
{
    IReadOnlyDictionary<BuildingGroup, CcBarracksViewModel> Create(IReadOnlyCollection<BuildingDto> buildings);
}
