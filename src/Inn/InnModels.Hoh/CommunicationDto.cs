using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class CommunicationDto
{
    // TODO move the rest of messages here, e.g. game design
    public PlayerRanksDTO PlayerRanks => PackedMessages.FindAndUnpack<PlayerRanksDTO>();
    public AllianceRanksDTO AllianceRanks => PackedMessages.FindAndUnpack<AllianceRanksDTO>();
    public HeroBattleStatsResponse HeroBattleStatsResponse => PackedMessages.FindAndUnpack<HeroBattleStatsResponse>();
    public PvpGetRankingResponse PvpGetRankingResponse => PackedMessages.FindAndUnpack<PvpGetRankingResponse>();
    public AllianceMembersResponse AllianceMembersResponse => PackedMessages.FindAndUnpack<AllianceMembersResponse>();
    public AlliancePush? AlliancePush => PackedMessages.FindAndUnpackToList<AlliancePush>().FirstOrDefault();

    public IList<HeroTreasureHuntPlayerPointsPush> HeroTreasureHuntPlayerPointsPushs =>
        PackedMessages.FindAndUnpackToList<HeroTreasureHuntPlayerPointsPush>();
    
    public IList<HeroTreasureHuntAlliancePointsPush> HeroTreasureHuntAlliancePointsPushs =>
        PackedMessages.FindAndUnpackToList<HeroTreasureHuntAlliancePointsPush>();
}
