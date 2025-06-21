using ProtoBuf;

namespace Ingweland.Fog.Models.Hoh.Entities.Battle;

[ProtoContract]
public class PvpResultPoints
{
    [ProtoMember(1)]
    public int PointsForEnemy { get; set; }

    [ProtoMember(2)]
    public int PointsForPlayer { get; set; }
}
