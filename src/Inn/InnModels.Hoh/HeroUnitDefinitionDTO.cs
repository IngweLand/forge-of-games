using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class HeroUnitDefinitionDTO
{
    public IList<HeroUnitStatValueDefinitionDTO> Stats => PackedStats.FindAndUnpackToList<HeroUnitStatValueDefinitionDTO>();
    
    public string? RarityId => PackedStats.Where(any => any.Is(GameDesignReference.Descriptor))
        .Select(any =>
        {
            var gameDesignReference = any.Unpack<GameDesignReference>();
            return gameDesignReference.Type.EndsWith("HeroUnitRarityDefinitionDTO") ? gameDesignReference.Id : null;
        }).SingleOrDefault(s => s != null);
}
