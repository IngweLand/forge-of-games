namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface IUndoableCommand
{
    void Execute();
    void Undo();
}
