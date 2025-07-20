using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class InGameEventDefinitionDto
{
    public IList<EventCityComponentDTO> EventCityComponents => PackedComponents.FindAndUnpackToList<EventCityComponentDTO>();
}
