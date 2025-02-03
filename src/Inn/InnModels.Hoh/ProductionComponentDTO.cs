namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class ProductionComponentDTO
{
    public WorkerBehaviourDTO? WorkerBehaviour => PackedWorkerBehaviour?.Unpack<WorkerBehaviourDTO>();

}
