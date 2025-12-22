using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class ResourceNotFoundError : Error
{
    public ResourceNotFoundError(string resourceId, string location) 
        : base($"Resource with ID '{resourceId}' was not found in {location}.")
    {
        Metadata.Add("ResourceId", resourceId);
        Metadata.Add("Location", location);
    }
}
