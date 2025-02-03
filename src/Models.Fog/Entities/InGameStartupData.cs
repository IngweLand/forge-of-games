namespace Ingweland.Fog.Models.Fog.Entities;

public class InGameStartupData
{
    public BasicCommandCenterProfile? Profile { get; init; }
    public IReadOnlyCollection<HohCity>? Cities { get; init; }
}
