using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class ResourceCollectionDto
{
    public IList<ResourceDTO> Resources => PackedResources.FindAndUnpackToList<ResourceDTO>();
}
