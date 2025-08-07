using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;

public interface ICommandCenterUiService
{
    Task<string> CreateProfileAsync(string profileName);
    Task<string> CreateProfileAsync(string profileName, BasicCommandCenterProfile profileDto);
    ValueTask<bool> DeleteProfileAsync(string profileId);
    Task EnsureInitializedAsync();
    Task<IReadOnlyCollection<CcProfileBasicsViewModel>> GetProfiles();
}
