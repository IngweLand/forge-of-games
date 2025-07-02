using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class CommunicationDto
{
    public AllianceMembersResponse AllianceMembersResponse => PackedMessages.FindAndUnpack<AllianceMembersResponse>();
    public AlliancePush? AlliancePush => PackedMessages.FindAndUnpackToList<AlliancePush>().FirstOrDefault();
    public AllianceRanksDTO AllianceRanks => PackedMessages.FindAndUnpack<AllianceRanksDTO>();
    public HeroBattleStatsResponse HeroBattleStatsResponse => PackedMessages.FindAndUnpack<HeroBattleStatsResponse>();

    public HeroFinishWaveResponse HeroFinishWaveResponse => PackedMessages.FindAndUnpack<HeroFinishWaveResponse>();
    public HeroStartBattleResponse HeroStartBattleResponse => PackedMessages.FindAndUnpack<HeroStartBattleResponse>();

    public IList<HeroTreasureHuntAlliancePointsPush> HeroTreasureHuntAlliancePointsPushs =>
        PackedMessages.FindAndUnpackToList<HeroTreasureHuntAlliancePointsPush>();

    public IList<HeroTreasureHuntPlayerPointsPush> HeroTreasureHuntPlayerPointsPushs =>
        PackedMessages.FindAndUnpackToList<HeroTreasureHuntPlayerPointsPush>();

    public OtherCityDTO OtherCity => PackedMessages.FindAndUnpack<OtherCityDTO>();

    // TODO move the rest of messages here, e.g. game design
    public PlayerRanksDTO PlayerRanks => PackedMessages.FindAndUnpack<PlayerRanksDTO>();
    public PvpGetRankingResponse PvpGetRankingResponse => PackedMessages.FindAndUnpack<PvpGetRankingResponse>();
}
