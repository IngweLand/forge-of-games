namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class BattleSquadDto
{
    public bool HasHero => Hero is {Properties: not null};
    public bool HasUnit => Unit is {Properties: not null};
}
