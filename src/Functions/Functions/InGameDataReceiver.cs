using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Functions.Validators;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Enums;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class InGameDataReceiver(
    ILogger<InGameDataReceiver> logger,
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    HohHelperResponseDtoToTablePkConverter hohHelperResponseDtoToTablePkConverter,
    DatabaseWarmUpService databaseWarmUpService,
    HohHelperResponseDtoValidator dtoValidator,
    IQueueRepository<InGameRawDataQueueMessage> queueRepository)
{
    [Function("InGameDataReceiver")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "hoh/inGameData")]
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

        List<( string ResponseUrl, string PartitionKey, string RowKey)> keys = [];
        Guid? submissionId = inGameData.SubmissionId != null ? Guid.Parse(inGameData.SubmissionId) : null;
        try
        {
            var now = DateTime.UtcNow;
            logger.LogInformation("Processing raw data at {Time}", now);
            var rawData = new InGameRawData
            {
                RequestBase64Data = inGameData.Base64RequestData,
                Base64Data = inGameData.Base64ResponseData!,
                SubmissionId = submissionId,
                CollectedAt = now,
            };
            var date = DateOnly.FromDateTime(now);
            foreach (var t in hohHelperResponseDtoToTablePkConverter.Get(inGameData, date))
            {
                var rowKey = await inGameRawDataTableRepository.SaveAsync(rawData, t.PartitionKey);
                keys.Add((inGameData.ResponseUrl, t.PartitionKey, rowKey));
                logger.LogInformation("Saved raw data for partition key: {PartitionKey}", t.PartitionKey);
            }

            logger.LogInformation("Processing raw data completed");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while processing raw data");
            throw;
        }

        try
        {
            foreach (var tuple in keys)
            {
                await QueueForImmediateProcessingIfRequired(submissionId, tuple.ResponseUrl, tuple.PartitionKey,
                    tuple.RowKey);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while queueing for immediate processing");
        }

        logger.LogInformation("Function InGameDataReceiver completed");
        return new NoContentResult();
    }

    private Task QueueForImmediateProcessingIfRequired(Guid? submissionId,
        string responseUrl, string partitionKey, string rowKey)
    {
        if (submissionId == null)
        {
            return Task.CompletedTask;
        }

        var processingServiceType = GetProcessingType(responseUrl);

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

    private static InGameDataProcessingServiceType GetProcessingType(string responseUrl)
    {
        var path = UriUtils.GetPath(responseUrl);

        return path switch
        {
            "game/battle/hero/complete-wave" => InGameDataProcessingServiceType.Battle,
            _ => InGameDataProcessingServiceType.Undefined,
        };
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
