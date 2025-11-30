using FluentResults;
using Ingweland.Fog.InnSdk.Hoh.Authentication.Models;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IRankingUpdateOrchestrator
{
    Task<Result> FetchAndStoreRankingAsync(GameWorldConfig gameWorld, PlayerRankingType rankingType);
}
