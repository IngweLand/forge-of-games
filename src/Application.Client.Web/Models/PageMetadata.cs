using Ingweland.Fog.Application.Core.Helpers;

namespace Ingweland.Fog.Application.Client.Web.Models;

public class PageMetadata
{
    public required string CurrentHomePath { get; init; }
    public required string Description { get; init; }
    public string HelpPagePath { get; init; } = FogUrlBuilder.PageRoutes.BASE_HELP_PATH;
    public string? Icon { get; init; }
    public required string Keywords { get; init; }
    public required string PageTitle { get; init; }
    public required string Title { get; init; }
}