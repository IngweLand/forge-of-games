using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcAddSharedProfilePage : AddRemoteProfilePageBase
{
    [Inject]
    private ICommandCenterSharingService CommandCenterRepository { get; set; }

    protected override bool CanGetProfile()
    {
        return !string.IsNullOrWhiteSpace(SharedProfileId);
    }

    protected override Task<BasicCommandCenterProfile?> GetProfile()
    {
        return CommandCenterRepository.GetSharedProfileAsync(SharedProfileId!);
    }
}

