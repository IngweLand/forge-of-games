using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Models;

public class CcBarracksViewModel
{
    public required BuildingGroup Group { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<int> Levels { get; init; }
    public int SelectedLevel { get; set; }
}
