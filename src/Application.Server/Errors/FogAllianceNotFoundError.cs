using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Errors;

public class FogAllianceNotFoundError : Error
{
    public FogAllianceNotFoundError(AllianceKey key) :
        base($"Alliance with key {key.WorldId}:{key.InGameAllianceId} not found.")
    {
        InGameAllianceId = key.InGameAllianceId;
        WorldId = key.WorldId;
        WithMetadata("WorldId", key.WorldId);
        WithMetadata("InGameAllianceId", key.InGameAllianceId);
    }

    public FogAllianceNotFoundError(int id) : base($"Alliance with id {id} not found.")
    {
        Id = id;
        WithMetadata("Id", id);
    }

    public int? Id { get; }

    public int? InGameAllianceId { get; }
    public string? WorldId { get; }
}
