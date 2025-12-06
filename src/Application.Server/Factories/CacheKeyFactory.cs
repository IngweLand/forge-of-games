using System.Globalization;
using Ingweland.Fog.Application.Server.Battle.Queries;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.PlayerCity.Queries;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Application.Server.StatsHub.Queries;
using Ingweland.Fog.Application.Server.StatsHub.Queries.Tops;

namespace Ingweland.Fog.Application.Server.Factories;

public class CacheKeyFactory : ICacheKeyFactory
{
    public string HeroDto(string heroId, Guid version)
    {
        return $"hero_dto:{heroId}:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string RelicDtos(Guid version)
    {
        return $"relic_dtos:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string HeroesBasicData(Guid version)
    {
        return $"heroes-basic-data:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string HohAges(Guid version)
    {
        return $"hoh-ages:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string PvpTiers(Guid version)
    {
        return $"pvp-tiers:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string TreasureHuntLeagues(Guid version)
    {
        return $"ath-leagues:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string HohResources(Guid version)
    {
        return $"hoh-resources:{CultureInfo.CurrentCulture.Name}:{version}";
    }

    public string Alliance(int allianceId)
    {
        return $"Alliance:{allianceId}";
    }

    public string CreateKey<TRequest>(TRequest request) where TRequest : ICacheableRequest
    {
        return request switch
        {
            GetAllianceQuery q => Alliance(q.AllianceId),
            BattleSearchQuery q => $"BattleSearch:{q.BattleDefinitionId}:{q.BattleType}:{q.ResultStatus}:{
                string.Join("-", q.UnitIds)}",
            GetBattleQuery q => $"Battle:{q.Id}",
            GetBattleStatsQuery q => $"BattleStats:{q.Id}:{CultureInfo.CurrentCulture.Name}",
            GetUnitBattlesQuery q => $"UnitBattles:{q.UnitId}:{q.BattleType}:{CultureInfo.CurrentCulture.Name}",
            CityInspirationsSearchQuery q => $"CityInspirationsSearch:{q.Request.CityId}:{q.Request.AgeId}:{
                q.Request.SearchPreference}:{q.Request.ProductionMetric}:{q.Request.AllowPremiumEntities}:{
                    q.Request.OpenedExpansionsHash}:{q.Request.TotalArea}",
            GetPlayerCityFromSnapshotQuery q => $"PlayerCityFromSnapshot:{q.SnapshotId}",
            GetPlayerCityQuery q => $"PlayerCity:{q.PlayerId}:{q.Date}",
            GetTopPlayersQuery q => $"TopPlayers:{q.WorldId}",
            GetTopAlliancesQuery q => $"TopAlliances:{q.WorldId}",
            GetPlayerBattlesQuery q => $"PlayerBattles:{q.PlayerId}:{q.StartIndex}:{q.Count}",
            GetPlayerProfileQuery q => $"PlayerProfile:{q.PlayerId}",
            GetPlayerQuery q => Player(q.PlayerId),
            GetTopHeroesQuery q => $"TopHeroes:{q.Mode}:{q.AgeId}:{q.FromLevel}:{q.ToLevel}",
            GetEquipmentInsightsQuery q => $"EquipmentInsights:{q.UnitId}",
            GetEquipmentDataQuery _ => $"EquipmentData:{CultureInfo.CurrentCulture.Name}",
            GetRelicInsightsQuery q => $"RelicInsights:{q.UnitId}",
            GetAllianceAthRankingsQuery q => $"AllianceAthRankings:{q.AllianceId}",
            GetAllianceRankingsQuery q => $"AllianceRankings:{q.AllianceId}",
            GetPlayerPvpRankingsQuery q => $"PlayerPvpRankings:{q.PlayerId}",
            GetPlayerRankingsQuery q => $"PlayerRankings:{q.PlayerId}",
            GetEventsQuery q => $"InGameEvents:{q.WorldId}:{q.EventDefinitionId}",
            GetAlliancesAthRankingsQuery q =>
                $"AlliancesAthRankings:{q.WorldId}:{q.League}:{q.StartIndex}:{q.PageSize}",
            GetEventCityRankingsQuery q => $"EventCityRankingsQuery:{q.WorldId}",
            GetAnnualBudgetQuery q => $"AnnualBudget:{q.Year}",
            _ => typeof(TRequest).FullName ?? Guid.NewGuid().ToString(),
        };
    }

    public string Player(int playerId)
    {
        return $"Player:{playerId}";
    }
}
