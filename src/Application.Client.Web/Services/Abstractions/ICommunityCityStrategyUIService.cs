using Ingweland.Fog.Application.Client.Web.ViewModels;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ICommunityCityStrategyUIService
{
    Task<IReadOnlyCollection<CommunityCityStrategyViewModel>> GetAllAsync();
}
