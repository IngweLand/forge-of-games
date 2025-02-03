using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Battle;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IEncounterViewService
{
    IList<EncounterRewardViewModel> GetRewards(Encounter encounter);
    IList<EncounterRewardViewModel> GetFirstTimeCompletionBonus(Encounter encounter);
}
