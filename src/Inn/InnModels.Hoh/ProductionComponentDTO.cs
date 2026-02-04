using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class ProductionComponentDTO
{
    public WorkerBehaviourDTO? WorkerBehaviour =>
        PackedBehaviours?.FindAndUnpackToList<WorkerBehaviourDTO>().FirstOrDefault();
}
