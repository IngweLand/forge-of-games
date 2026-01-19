using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class FogPlayerNotFoundError : Error
{
    public FogPlayerNotFoundError(int id) : base($"Player with id {id} not found.")
    {
        Id = id;
        WithMetadata("Id", id);
    }

    public int? Id { get; }
}
