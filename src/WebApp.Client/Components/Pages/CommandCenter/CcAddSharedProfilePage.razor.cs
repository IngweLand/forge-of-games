using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.WebApp.Client.Components.Elements;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.CommandCenter;

public partial class CcAddSharedProfilePage : AddRemoteProfilePageBase
{
    [Inject]
    private ICommandCenterProfileSharingService CommandCenterProfileRepository { get; set; }

    protected override bool CanGetProfile()
    {
        return !string.IsNullOrWhiteSpace(SharedProfileId);
    }

    protected override Task<BasicCommandCenterProfile?> GetProfile()
    {
        return CommandCenterProfileRepository.GetSharedProfileAsync(SharedProfileId!);
    }
}

