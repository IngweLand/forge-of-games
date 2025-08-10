using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class SearchAllianceResponseNotFoundError : Error
{
    public string SearchString { get; }
    public string WorldId { get; }

    public SearchAllianceResponseNotFoundError(string searchString, string worldId) :
        base($"Alliance with ID {searchString} not found in world {worldId}.")
    {
        SearchString = searchString;
        WorldId = worldId;
        WithMetadata("SearchString", searchString);
        WithMetadata("WorldId", worldId);
    }
}
