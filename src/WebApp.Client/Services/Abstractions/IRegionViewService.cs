using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IRegionViewService
{
    IList<IconLabelItemViewModel> GetRewards(Region region);
    IList<EncounterViewModel> GetEncounters(Region region);
}
