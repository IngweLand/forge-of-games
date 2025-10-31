using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Functions.Data;
using Ingweland.Fog.Functions.Validators;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Enums;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class InGameDataParseFunction(
    ILogger<InGameDataParseFunction> logger,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    HohHelperResponseDtoToTablePkConverter hohHelperResponseDtoToTablePkConverter,
    DatabaseWarmUpService databaseWarmUpService,
    HohHelperResponseDtoValidator dtoValidator,
    IQueueRepository<InGameRawDataQueueMessage> queueRepository,
    IInGameDataParsingService inGameDataParsingService)
{
    [Function("InGameDataParseFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hoh/inGameData/parse")]
        HttpRequest req,
        [Microsoft.Azure.Functions.Worker.Http.FromBody]
        HohHelperResponseDto inGameData)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();

        SetDebugCorsHeaders(req);

        if (!dtoValidator.Validate(inGameData, out var error))
        {
            logger.LogError("Dto validation failed: {error}", error);

            return new BadRequestResult();
        }

        IReadOnlyCollection<PlayerAthPointsDto> parsedData = [];
        try
        {
            var now = DateTime.UtcNow;
            logger.LogInformation("Processing raw data at {Time}", now);
            var rawData = new InGameRawData
            {
                Base64Data = inGameData.Base64ResponseData!,
                CollectedAt = now,
            };
            var date = DateOnly.FromDateTime(now);
            var pks = hohHelperResponseDtoToTablePkConverter.Get(inGameData, date);

            foreach (var t in pks)
            {
                switch (t.ProcessingServiceType)
                {
                    case InGameDataProcessingServiceType.WakeupLeaderboards:
                    {
                        await Save(rawData, t.PartitionKey, InGameDataProcessingServiceType.WakeupLeaderboards);
                        break;
                    }

                    case InGameDataProcessingServiceType.WakeupAlliance:
                    {
                        parsedData = ParsePlayerAthRankings(rawData);
                        logger.LogInformation("Parsed player ATH rankings");
                        break;
                    }
                }
            }

            logger.LogInformation("Processing raw data completed");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while processing raw data");
            throw;
        }

        return new OkObjectResult(parsedData);
    }

    private IReadOnlyCollection<PlayerAthPointsDto> ParsePlayerAthRankings(InGameRawData rawData)
    {
        var wakeup = inGameDataParsingService.ParseWakeup(rawData.Base64Data);
        return wakeup.AthPlayerRankings.Select(x => new PlayerAthPointsDto
        {
            EventId = x.TreasureHuntEventId,
            Points = x.RankingPoints,
            PlayerId = x.Player.Id,
            PlayerName = x.Player.Name,
        }).ToList();
    }

    private async Task Save(InGameRawData rawData, string partitionKey,
        InGameDataProcessingServiceType processingServiceType)
    {
        string rowKey;
        try
        {
            rowKey = await inGameRawDataTableRepository.SaveAsync(rawData, partitionKey);
            logger.LogInformation("Saved raw data for partition key: {PartitionKey}", partitionKey);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while processing raw data");
            return;
        }

        try
        {
            await QueueForImmediateProcessingIfRequired(processingServiceType, partitionKey,
                rowKey);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while queueing for immediate processing");
        }
    }

    private Task QueueForImmediateProcessingIfRequired(InGameDataProcessingServiceType processingServiceType,
        string partitionKey, string rowKey)
    {
        if (processingServiceType != InGameDataProcessingServiceType.Undefined)
        {
            return queueRepository.SendMessageAsync(new InGameRawDataQueueMessage
            {
                ProcessingServiceType = processingServiceType,
                PartitionKey = partitionKey,
                RowKey = rowKey,
            });
        }

        return Task.CompletedTask;
    }

    private void SetDebugCorsHeaders(HttpRequest req)
    {
#if DEBUG
        // Set CORS headers
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Methods", "POST, OPTIONS");
        req.HttpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
#endif
    }
}
