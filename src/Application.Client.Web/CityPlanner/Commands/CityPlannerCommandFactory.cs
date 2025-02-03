using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;

public class CityPlannerCommandFactory(ICityPlanner cityPlanner) :ICityPlannerCommandFactory
{
    public IUndoableCommand CreateAddBuildingCommand(BuildingGroup buildingGroup)
    {
        return new AddEntityCommand(cityPlanner, buildingGroup);
    }
    public IUndoableCommand CreateDeleteEntityCommand(CityMapEntity entity)
    {
        return new DeleteEntityCommand(cityPlanner, entity);
    }

    public IUndoableCommand CreateRotateEntityCommand(CityMapEntity entity)
    {
        return new RotateEntityCommand(cityPlanner, entity);
    }

    public IUndoableCommand CreateMoveEntityCommand(CityMapEntity entity, Point oldLocation)
    {
        return new MoveEntityCommand(cityPlanner, entity, oldLocation);
    }
    
    public IUndoableCommand CreateChangeEntityLevelCommand(CityMapEntity entity, int level)
    {
        return new ChangeEntityLevelCommand(cityPlanner, entity, level);
    }
}
