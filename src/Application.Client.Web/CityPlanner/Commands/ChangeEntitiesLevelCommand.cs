using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class ChangeEntitiesLevelCommand(
    ICityPlanner cityPlanner,
    IReadOnlyCollection<CityMapEntity> entities,
    int level) : IUndoableCommand
{
    private readonly IReadOnlyDictionary<int, int> _entityIdToLevelMap =
        entities.ToDictionary(src => src.Id, _ => level);

    private readonly IReadOnlyDictionary<int, int> _entityIdToOldLevelMap =
        entities.ToDictionary(src => src.Id, src => src.Level);

    public void Execute()
    {
        cityPlanner.UpdateLevels(_entityIdToLevelMap);
    }

    public void Undo()
    {
        cityPlanner.UpdateLevels(_entityIdToOldLevelMap);
    }
}