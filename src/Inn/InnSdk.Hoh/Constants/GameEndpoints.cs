namespace Ingweland.Fog.InnSdk.Hoh.Constants;

public static class GameEndpoints
{
    public static readonly string WebLoginUrl = "https://{0}.heroesgame.com/api/login";
    public static readonly string AccountPlayUrl = "https://{0}0.heroesofhistorygame.com/core/api/account/play";
    public static readonly string BaseApiUrl = "https://{0}.heroesofhistorygame.com";
    public static readonly string LocaPath = "game/loca";
    public static readonly string GameDesignPath = "game/gamedesign";
    public static readonly string PlayerRankingPath = "game/ranking/player";
    public static readonly string AllianceRankingPath = "game/ranking/alliance";

    public static string CreateUrl(string serverId, string path)
    {
        var baseUrl = string.Format(BaseApiUrl, serverId);
        return $"{baseUrl}/{path}";
    }
}
