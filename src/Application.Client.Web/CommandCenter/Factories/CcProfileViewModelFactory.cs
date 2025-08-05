using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class CcProfileViewModelFactory(ICcProfileTeamViewModelFactory teamViewModelFactory) : ICcProfileViewModelFactory
{
    public CcProfileViewModel Create(CommandCenterProfile profile,
        IReadOnlyDictionary<string, HeroProfileViewModel> heroes)
    {
        var teams = profile.Teams.Values.Select(t => teamViewModelFactory.Create(t, heroes)).ToList();
        return new CcProfileViewModel()
        {
            Id = profile.Id,
            Name = profile.Name,
            Teams = teams,
        };
    }
}
