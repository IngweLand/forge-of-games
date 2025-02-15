using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.WebApp.Client.Models;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IPageSetupService
{
    string GetCurrentHomePath();
    IReadOnlyCollection<NavMenuItem>? GetCurrentSectionMenuItems();
    string GetHelpUrl();
    IReadOnlyCollection<NavMenuItem> GetMainMenuItems();
    PageMetadata GetMetadata();
    string? GetPageIcon();
}
