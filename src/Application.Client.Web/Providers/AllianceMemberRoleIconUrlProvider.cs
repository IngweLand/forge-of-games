using System.Globalization;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class AllianceMemberRoleIconUrlProvider(IAssetUrlProvider assetUrlProvider) : IAllianceMemberRoleIconUrlProvider
{
    public string? GetIconUrl(AllianceMemberRole role)
    {
        return role switch
        {
            AllianceMemberRole.AllianceMinister or AllianceMemberRole.AllianceLeader => 
                assetUrlProvider.GetHohIconUrl($"icon_alliance_title_{role.ToString().ToLowerInvariant()}"),
            _ => null,
        };
    }
}
