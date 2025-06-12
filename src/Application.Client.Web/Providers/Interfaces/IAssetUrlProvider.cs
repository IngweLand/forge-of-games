using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IAssetUrlProvider
{
    string GetHohHeroAbilityIconUrl(string heroAbilityId);
    string GetHohIconUrl(string assetId, string extension);
    string GetHohIconUrl(string assetId);
    string GetHohImageUrl(string assetId, string extension);
    string GetHohImageUrl(string assetId);
    string GetHohPlayerAvatarUrl(string avatarId);
    string GetHohTechnologyImageUrl(string assetId, string extension);
    string GetHohTechnologyImageUrl(string assetId);
    string GetHohUnitImageUrl(string assetId);
    string GetHohUnitPortraitUrl(string assetId, string extension);
    string GetHohUnitPortraitUrl(string assetId);
    string GetHohUnitStatIconUrl(UnitStatType unitStatType);
    string GetHohUnitVideoUrl(string assetId, string extension);
    string GetHohUnitVideoUrl(string assetId);
    string GetNotoSansFontUrl(string locale);
    string GetImageUrl(string filename);
    string GetHohEquipmentSetIconUrl(EquipmentSet equipmentSet);
}