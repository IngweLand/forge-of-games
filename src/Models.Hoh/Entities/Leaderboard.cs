using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Models.Hoh.Entities;

public class Leaderboard
{
    public IReadOnlyCollection<(HohPlayer Player, HohAlliance? Alliance)> Participants { get; init; } = [];
}
