using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICcProfileViewModelFactory
{
    CcProfileViewModel Create(CommandCenterProfile profile,
        IReadOnlyDictionary<string, HeroProfileViewModel> heroes);
}
