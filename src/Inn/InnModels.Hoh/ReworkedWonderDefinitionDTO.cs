using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class ReworkedWonderDefinitionDTO
{
    public WonderLevelUpComponentDTO LevelUpComponent => PackedLevels.FindAndUnpackToList<WonderLevelUpComponentDTO>().Single();
    public IList<GrantWorkerComponentDTO> GrantWorkerComponents => PackedLevels.FindAndUnpackToList<GrantWorkerComponentDTO>();
    public IList<BoostResourceComponentDTO> BoostResourceComponents => PackedLevels.FindAndUnpackToList<BoostResourceComponentDTO>();
}
