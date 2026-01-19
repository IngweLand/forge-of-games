using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class EntityNotFoundError : Error
{
    public EntityNotFoundError(string entityName, object id) 
        : base($"{entityName} with ID '{id}' was not found.")
    {
        Metadata.Add("EntityType", entityName);
        Metadata.Add("Id", id);
    }
}
