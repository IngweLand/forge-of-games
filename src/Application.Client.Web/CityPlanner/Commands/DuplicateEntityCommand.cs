using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class DuplicateEntityCommand(ICityPlanner cityPlanner, int cityMapEntityId) : IUndoableCommand
{
    private int _newCityMapEntityId;

    public void Execute()
    {
        var newEntity = cityPlanner.DuplicateEntity(cityMapEntityId);
        _newCityMapEntityId = newEntity?.Id ?? 0;
    }

    public void Undo()
    {
        cityPlanner.DeleteEntity(_newCityMapEntityId);
    }
}
