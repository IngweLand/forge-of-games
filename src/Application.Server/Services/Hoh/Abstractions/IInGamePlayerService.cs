using FluentResults;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;

namespace Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;

public interface IInGamePlayerService
{
    Task<Result<PlayerProfile>> FetchProfile(PlayerKey playerKey);
}
