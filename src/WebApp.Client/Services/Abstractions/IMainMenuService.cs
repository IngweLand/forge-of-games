using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.WebApp.Client.Models;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IMainMenuService
{
    IReadOnlyCollection<NavMenuItem> GetMainMenuItems();
}
