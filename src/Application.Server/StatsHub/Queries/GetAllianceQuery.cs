using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Application.Server.StatsHub.Factories;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.StatsHub.Queries;

public record GetAllianceQuery : IRequest<AllianceProfileDto?>, ICacheableRequest
{
    public required int AllianceId { get; init; }
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetAllianceQueryHandler(
    IFogDbContext context,
    IAllianceProfileDtoFactory allianceProfileDtoFactory,
    IAllianceUpdateOrchestrator allianceUpdateOrchestrator,
    ILogger<GetAllianceQueryHandler> logger)
    : IRequestHandler<GetAllianceQuery, AllianceProfileDto?>
{
    public async Task<AllianceProfileDto?> Handle(GetAllianceQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Getting alliance: {AllianceId}", request.AllianceId);
        var existingAlliance = await context.Alliances.FindAsync(request.AllianceId, cancellationToken);
        if (existingAlliance == null)
        {
            logger.LogInformation("Alliance with ID {AllianceId} not found", request.AllianceId);
            return null;
        }

        var minDate = DateTime.UtcNow.AddDays(FogConstants.ALLIANCE_STALE_THRESHOLD_DAYS * -1);
        var shouldUpdateMembers = existingAlliance.MembersUpdatedAt < minDate &&
            existingAlliance.Status == InGameEntityStatus.Active;
        if (existingAlliance.UpdatedAt < minDate.ToDateOnly() && existingAlliance.Status == InGameEntityStatus.Active)
        {
            logger.LogInformation(
                "Alliance needs update (Alliance ID: {AllianceId}, UpdatedAt: {UpdatedAt}, Minimum : {Today})",
                request.AllianceId, existingAlliance.UpdatedAt, minDate);
            var updateResult = await allianceUpdateOrchestrator.UpdateAsync(existingAlliance.Id, cancellationToken);
            updateResult.LogIfFailed<GetAllianceQueryHandler>();
            if (updateResult.IsFailed)
            {
                shouldUpdateMembers = false;
            }
        }

        if (shouldUpdateMembers)
        {
            logger.LogInformation(
                "Alliance members need update (Alliance ID: {AllianceId}, UpdatedAt: {UpdatedAt}, Minimum : {Today})",
                request.AllianceId, existingAlliance.MembersUpdatedAt, minDate);
            var membersUpdateResult =
                await allianceUpdateOrchestrator.UpdateMembersAsync(existingAlliance.Id, cancellationToken);
            membersUpdateResult.LogIfFailed<GetAllianceQueryHandler>();
        }

        var alliance = await context.Alliances.AsNoTracking()
            .Include(p => p.Members.Where(x => x.Player.Status == InGameEntityStatus.Active)).ThenInclude(x => x.Player)
            .Include(p => p.NameHistory)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.AllianceId, cancellationToken);
        return alliance == null ? null : allianceProfileDtoFactory.Create(alliance);
    }
}
