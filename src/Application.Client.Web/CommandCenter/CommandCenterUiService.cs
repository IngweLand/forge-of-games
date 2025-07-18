using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Caching.Interfaces;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter;

public class CommandCenterUiService(
    ICommandCenterService commandCenterService,
    IPersistenceService persistenceService,
    IHohCommandCenterProfileFactory commandCenterProfileFactory,
    IHohCoreDataCache coreDataCache,
    IMapper mapper) : ICommandCenterUiService
{
    private bool _isInitialized;

    public async Task<string> CreateProfileAsync(string profileName)
    {
        await EnsureInitializedAsync();

        var profile = commandCenterProfileFactory.Create(profileName);
        await persistenceService.SaveCommandCenterProfile(mapper.Map<BasicCommandCenterProfile>(profile));
        return profile.Id;
    }

    public async Task<string> CreateProfileAsync(string profileName, BasicCommandCenterProfile profileDto)
    {
        await EnsureInitializedAsync();

        var clone = mapper.Map<BasicCommandCenterProfile>(profileDto);
        clone.Name = profileName;
        await persistenceService.SaveCommandCenterProfile(clone);
        return clone.Id;
    }

    public ValueTask<bool> DeleteProfileAsync(string profileId)
    {
        return persistenceService.DeleteProfile(profileId);
    }

    public async Task<IReadOnlyCollection<CcProfileBasicsViewModel>> GetProfiles()
    {
        await EnsureInitializedAsync();
        var profiles = await persistenceService.GetProfilesAsync();
        return mapper.Map<IReadOnlyCollection<CcProfileBasicsViewModel>>(profiles);
    }

    public async Task EnsureInitializedAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        var data = await commandCenterService.GetCommandCenterDataAsync();
        await coreDataCache.AddHeroesAsync(data.Heroes);
        await coreDataCache.AddBarracksAsync(data.Barracks);
        _isInitialized = true;
    }
}
