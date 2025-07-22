using System.Drawing;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICityPlannerCommandFactory
{
    IUndoableCommand CreateAddBuildingCommand(BuildingGroup buildingGroup);
    IUndoableCommand CreateDeleteEntityCommand(CityMapEntity entity);
    IUndoableCommand CreateRotateEntityCommand(int cityMapEntityId);
    IUndoableCommand CreateDuplicateEntityCommand(int cityMapEntityId);
    IUndoableCommand CreateMoveToInventoryCommand(IReadOnlySet<int> cityMapEntityIds);
    IUndoableCommand CreateMoveAllToInventoryCommand();
    IUndoableCommand CreatePurgeInventoryCommand();
    IUndoableCommand CreateMoveFromInventoryCommand(BuildingGroup buildingGroup);
    IUndoableCommand CreateMoveEntityCommand(CityMapEntity entity, Point oldLocation);
    IUndoableCommand CreateChangeEntityLevelCommand(CityMapEntity entity, int level);
    IUndoableCommand CreateChangeEntitiesLevelCommand(IReadOnlyCollection<CityMapEntity> entities, int level);
}
