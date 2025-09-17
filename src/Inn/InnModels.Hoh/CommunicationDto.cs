using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class CommunicationDto
{
    public AllianceMembersResponse AllianceMembersResponse
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpack<AllianceMembersResponse>();
            }
            catch
            {
                // ignore
            }

            return PackedMessages7.FindAndUnpack<AllianceMembersResponse>();
        }
    }

    public AlliancePush? AlliancePush
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpackToList<AlliancePush>().FirstOrDefault();
            }
            catch
            {
                // ignore
            }

            return PackedMessages7.FindAndUnpackToList<AlliancePush>().FirstOrDefault();
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

            return PackedMessages7.FindAndUnpack<AllianceRanksDTO>();
        }
    }

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

            return PackedMessages7.FindAndUnpack<HeroBattleStatsResponse>();
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

            return PackedMessages7.FindAndUnpack<HeroFinishWaveResponse>();
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

            return PackedMessages7.FindAndUnpack<HeroStartBattleResponse>();
        }
    }

    public IList<HeroTreasureHuntAlliancePointsPush> HeroTreasureHuntAlliancePointsPushs
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpackToList<HeroTreasureHuntAlliancePointsPush>();
            }
            catch
            {
                // ignore
            }

            return PackedMessages7.FindAndUnpackToList<HeroTreasureHuntAlliancePointsPush>();
        }
    }

    public IList<HeroTreasureHuntPlayerPointsPush> HeroTreasureHuntPlayerPointsPushs
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpackToList<HeroTreasureHuntPlayerPointsPush>();
            }
            catch
            {
                // ignore
            }

            return PackedMessages7.FindAndUnpackToList<HeroTreasureHuntPlayerPointsPush>();
        }
    }

    public LeaderboardPush? LeaderboardPush
    {
        get
        {
            try
            {
                return PackedMessages.FindAndUnpackToList<LeaderboardPush>().FirstOrDefault();
            }
            catch
            {
                // ignore
            }

            return PackedMessages7.FindAndUnpackToList<LeaderboardPush>().FirstOrDefault();
        }
    }

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

            return PackedMessages7.FindAndUnpack<OtherCityDTO>();
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

            return PackedMessages7.FindAndUnpack<PlayerRanksDTO>();
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

            return PackedMessages7.FindAndUnpack<PvpGetRankingResponse>();
        }
    }
}