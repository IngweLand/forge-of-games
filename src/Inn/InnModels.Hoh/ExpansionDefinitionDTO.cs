using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class ExpansionDefinitionDTO
{
    public LinkedExpansionComponentDTO? LinkedExpansionComponent =>
        PackedComponents.FindAndUnpackToList<LinkedExpansionComponentDTO>().FirstOrDefault();
}
