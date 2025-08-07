using System.Text.Json;
using Azure.Storage.Queues.Models;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Functions.Services;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Infrastructure.Enums;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class InGameDataQueueProcessor(
    IInGameRawDataTableRepository inGameRawDataTableRepository,
    IInGameDataParsingService inGameDataParsingService,
    IBattleService battleService,
    DatabaseWarmUpService databaseWarmUpService,
    ILogger<InGameDataQueueProcessor> logger)
{
    [Function(nameof(InGameDataQueueProcessor))]
    public async Task Run(
        [QueueTrigger("%StorageSettings:InGameRawDataProcessingQueue%",
            Connection = "StorageSettings:ConnectionString")]
        QueueMessage message)
    {
        logger.LogInformation("Message: {MessageId}, payload: {MessageMessageText}", message.MessageId,
            message.MessageText);
        var payload = JsonSerializer.Deserialize<InGameRawDataQueueMessage>(message.MessageText);
        if (payload == null)
        {
            throw new Exception("Could not deserialize payload");
        }

        switch (payload.ProcessingServiceType)
        {
            case InGameDataProcessingServiceType.Battle:
            {
                await ProcessBattleAsync(payload.PartitionKey, payload.RowKey);
                break;
            }
            default:
            {
                throw new Exception($"Unknown processing service type {payload.ProcessingServiceType}");
            }
        }

        logger.LogInformation("Successfully processed: {MessageId}", message.MessageId);
    }

    private async Task ProcessBattleAsync(string partitionKey, string rowKey)
    {
        await databaseWarmUpService.WarmUpDatabaseIfRequiredAsync();
        
        var rawData = await inGameRawDataTableRepository.GetAsync(partitionKey, rowKey);
        if (rawData == null)
        {
            throw new Exception($"Could not find raw data for partition key {partitionKey} and row key {rowKey}");
        }

        var parsed = inGameDataParsingService.ParseBattleWaveResult(rawData.Base64Data);

        if (parsed.ResultStatus == BattleResultStatus.Undefined)
        {
            logger.LogInformation("Skipping because result status of the battles is Undefined");
            return;
        }

        var parts = partitionKey.Split('_');
        await battleService.AddAsync(parts[1], parsed, DateOnly.ParseExact(parts[2], "yyyy-MM-dd"),
            rawData.SubmissionId);
    }
}
