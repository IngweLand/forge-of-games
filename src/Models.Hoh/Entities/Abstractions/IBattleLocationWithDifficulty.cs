using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Models.Hoh.Entities.Abstractions;

public interface IBattleLocationWithDifficulty
{
    public Difficulty Difficulty { get; set; }
}
