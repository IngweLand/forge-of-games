using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class ResourceLocalizationService(IStringLocalizer<FogResource> localizer) : IResourceLocalizationService
{
    public string Localize(BattleType src)
    {
        return src switch
        {
            BattleType.Campaign => localizer[FogResource.BattleType_Campaign],
            BattleType.HistoricBattle => localizer[FogResource.BattleType_HistoricBattle],
            BattleType.Pvp => localizer[FogResource.BattleType_PvP],
            BattleType.TeslaStorm => localizer[FogResource.BattleType_TeslaStorm],
            BattleType.TreasureHunt => localizer[FogResource.BattleType_TreasureHunt],
            BattleType.AncientEgypt => localizer[FogResource.BattleType_AncientEgypt],
            _ => string.Empty,
        };
    }
}
