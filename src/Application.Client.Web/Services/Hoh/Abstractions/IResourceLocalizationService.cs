using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IResourceLocalizationService
{
    string Localize(BattleType src);
}
