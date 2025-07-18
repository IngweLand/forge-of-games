using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class CcProfileTeamViewModelFactory : ICcProfileTeamViewModelFactory
{
    public CcProfileTeamViewModel Create(CommandCenterProfileTeam team,
        IReadOnlyDictionary<string, HeroProfileViewModel> viewModels)
    {
        if (team.HeroIds.Any(id => !viewModels.ContainsKey(id)))
        {
            throw new ArgumentException("One or more hero profile IDs not found in view models dictionary",
                nameof(viewModels));
        }

        var heroes = team.HeroIds.Select(id => viewModels[id]).ToList();
        var power = heroes.Sum(hp => hp.TotalPower);
        return new CcProfileTeamViewModel()
        {
            Id = team.Id,
            Name = team.Name,
            Heroes = heroes,
            Power = power != 0 ? power.ToString() : string.Empty,
        };
    }
}
