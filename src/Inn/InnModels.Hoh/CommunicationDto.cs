using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class CommunicationDto
{
    // TODO move the rest of messages here, e.g. game design
    public PlayerRanksDTO PlayerRanks => PackedMessages.FindAndUnpack<PlayerRanksDTO>();
    public AllianceRanksDTO AllianceRanks => PackedMessages.FindAndUnpack<AllianceRanksDTO>();
}
