using Ingweland.Fog.Application.Client.Web.Localization;
using Microsoft.AspNetCore.Components.Routing;

namespace Ingweland.Fog.WebApp.Client.Models;

public class NavMenuItem
{
    public IReadOnlyCollection<NavMenuItem> Children { get; init; } = [];
    public bool Expanded { get; init; } = true;
    public string? Href { get; init; }
    public string? Icon { get; init; }
    public bool IsGroup => Children.Count != 0;
    public NavLinkMatch Match { get; init; } = NavLinkMatch.Prefix;
    public required string ResourceKey { get; init; }
}
