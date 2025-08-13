using FluentResults;
using Ingweland.Fog.Models.Fog.Enums;

namespace Ingweland.Fog.Application.Server.Services.Interfaces;

public interface IHeroInsightsService
{
    Task<Result<IReadOnlySet<string>>> GetAsync(HeroInsightsMode mode, string? ageId, int? fromLevel,
        int? toLevel, CancellationToken cancellationToken);
}
