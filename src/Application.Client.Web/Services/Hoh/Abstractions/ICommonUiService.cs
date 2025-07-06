using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface ICommonUiService
{
    Task<IReadOnlyDictionary<string, AgeViewModel>> GetAgesAsync();
}
