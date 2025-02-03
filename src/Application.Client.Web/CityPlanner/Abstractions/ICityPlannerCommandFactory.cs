using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Commands;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerCommandFactory
{
    IUndoableCommand CreateAddBuildingCommand(BuildingGroup buildingGroup);
    IUndoableCommand CreateDeleteEntityCommand(CityMapEntity entity);
    IUndoableCommand CreateRotateEntityCommand(CityMapEntity entity);
    IUndoableCommand CreateMoveEntityCommand(CityMapEntity entity, Point oldLocation);
    IUndoableCommand CreateChangeEntityLevelCommand(CityMapEntity entity, int level);
}
