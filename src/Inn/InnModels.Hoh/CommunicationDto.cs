using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class CommunicationDto
{
    public AllianceMembersResponse? AllianceMembersResponse =>
        RootContext.Messages.FindAndUnpackToList<AllianceMembersResponse>().FirstOrDefault();

    public AlliancePush? AlliancePush
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<AlliancePush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<AlliancePush>();
            }

            return items.FirstOrDefault();
        }
    }

    public AllianceRanksDTO AllianceRanks
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<AllianceRanksDTO>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<AllianceRanksDTO>();
        }
    }

    public IList<CityDTO> Cities
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<CityDTO>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<CityDTO>();
            }

            return items;
        }
    }

    public EquipmentPush? Equipment
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<EquipmentPush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<EquipmentPush>();
            }

            return items.FirstOrDefault();
        }
    }

    public GameDesignResponse GameDesignResponse => Response.FindAndUnpack<GameDesignResponse>();

    public HeroBattleStatsResponse HeroBattleStatsResponse
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<HeroBattleStatsResponse>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<HeroBattleStatsResponse>();
        }
    }

    public HeroFinishWaveResponse HeroFinishWaveResponse
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<HeroFinishWaveResponse>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<HeroFinishWaveResponse>();
        }
    }

    public HeroPush HeroPush
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<HeroPush>();
            }
            catch
            {
                // ignore
            }

            return RootContext.Messages.FindAndUnpack<HeroPush>();
        }
    }

    public HeroStartBattleResponse HeroStartBattleResponse
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<HeroStartBattleResponse>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<HeroStartBattleResponse>();
        }
    }

    public IList<HeroTreasureHuntAlliancePointsPush> HeroTreasureHuntAlliancePointsPushs
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<HeroTreasureHuntAlliancePointsPush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<HeroTreasureHuntAlliancePointsPush>();
            }

            return items;
        }
    }

    public IList<HeroTreasureHuntPlayerPointsPush> HeroTreasureHuntPlayerPointsPushs
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<HeroTreasureHuntPlayerPointsPush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<HeroTreasureHuntPlayerPointsPush>();
            }

            return items;
        }
    }

    public IList<InGameEventDto> InGameEvents
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<InGameEventPush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<InGameEventPush>();
            }

            return items.FirstOrDefault()?.Events ?? [];
        }
    }

    public IList<LeaderboardPush> Leaderboards
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<LeaderboardPush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<LeaderboardPush>();
            }

            return items;
        }
    }

    public LocaResponse LocaResponse => Response.FindAndUnpack<LocaResponse>();

    public OtherCityDTO OtherCity
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<OtherCityDTO>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<OtherCityDTO>();
        }
    }

    public PlayerRanksDTO PlayerRanks
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<PlayerRanksDTO>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<PlayerRanksDTO>();
        }
    }

    public PvpBattleHistoryResponse PvpBattleHistoryResponse
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<PvpBattleHistoryResponse>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<PvpBattleHistoryResponse>();
        }
    }

    public PvpGetRankingResponse PvpGetRankingResponse
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<PvpGetRankingResponse>();
            }
            catch
            {
                // ignore
            }

            return Response.FindAndUnpack<PvpGetRankingResponse>();
        }
    }

    public RelicPush? RelicPush
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<RelicPush>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<RelicPush>();
            }

            return items.FirstOrDefault();
        }
    }

    public ResearchStateDTO? ResearchState
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<ResearchStateDTO>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<ResearchStateDTO>();
            }

            return items.FirstOrDefault();
        }
    }
    
    public BatchResponse BatchResponse => Response.FindAndUnpack<BatchResponse>();

    public ReworkedWondersDTO? Wonders
    {
        get
        {
            var items = PackedMessages.FindAndUnpackToList<ReworkedWondersDTO>();
            if (items.Count == 0)
            {
                items = RootContext.Messages.FindAndUnpackToList<ReworkedWondersDTO>();
            }

            return items.FirstOrDefault();
        }
    }
}
