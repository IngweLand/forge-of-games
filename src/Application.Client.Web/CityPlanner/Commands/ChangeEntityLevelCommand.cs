using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class ChangeEntityLevelCommand(ICityPlanner cityPlanner, CityMapEntity entity, int level) :IUndoableCommand
{
    private readonly int _oldLevel = entity.Level;
    private CityMapEntity? _newEntity;
    public void Execute()
    {
        _newEntity = cityPlanner.UpdateLevel(entity, level);
    }

    public void Undo()
    {
        if (_newEntity != null)
        {
            cityPlanner.UpdateLevel(_newEntity, _oldLevel);
        }
    }
}
