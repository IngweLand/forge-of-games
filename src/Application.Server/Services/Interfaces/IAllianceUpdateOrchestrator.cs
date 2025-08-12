using FluentResults;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IAllianceUpdateOrchestrator
{
    Task<Result> UpdateMembersAsync(int id, CancellationToken ct);
}
