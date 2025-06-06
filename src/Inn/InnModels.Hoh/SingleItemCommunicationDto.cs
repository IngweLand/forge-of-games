namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class SingleItemCommunicationDto
{
    public PvpBattleHistoryResponseDto? PvpBattleHistoryResponse =>
        PvpBattleHistoryResponseDto.Parser.ParseFrom(PackedMessage.Value);
}