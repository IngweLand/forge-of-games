namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ICommandManager
{
    bool CanRedo { get; }
    bool CanUndo { get; }
    void ExecuteCommand(IUndoableCommand command);
    void Undo();
    void Redo();
    void Reset();
}
