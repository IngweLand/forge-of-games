using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICcProfileTeamViewModelFactory
{
    CcProfileTeamViewModel Create(CommandCenterProfileTeam team,
        IReadOnlyDictionary<string, HeroProfileViewModel> viewModels);
}
