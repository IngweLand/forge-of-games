using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Models;

public sealed class AppBarState
{
    public RenderFragment? Content { get; set; }
    public bool ShouldHideTitle { get; set; }
}
