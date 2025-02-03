using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class ResearchDetailsDto
{
    public IList<ResourceDTO> Costs => PackedCosts.FindAndUnpackToList<ResourceDTO>();

    public IList<ResearchRequirementDTO> ResearchRequirements =>
        PackedResearchRequirements.FindAndUnpackToList<ResearchRequirementDTO>();
}
