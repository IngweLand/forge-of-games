using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class BuildingRequirementsDto
{
    public IList<ResourceDTO> Cost => PackedCost.FindAndUnpackToList<ResourceDTO>();
}
