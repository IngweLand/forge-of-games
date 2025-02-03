namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class TechnologyDefinitionDTO
{
    public ResearchComponentDTO ResearchComponent => PackedResearchComponent.Unpack<ResearchComponentDTO>();
}
