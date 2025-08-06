using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Functions.Validators;
using Ingweland.Fog.Models.Fog.Entities;
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
    HohHelperResponseDtoValidator dtoValidator)
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

        try
        {
            var now = DateTime.UtcNow;
            logger.LogInformation("Processing raw data at {Time}", now);
            var rawData = new InGameRawData
            {
                RequestBase64Data = inGameData.Base64RequestData,
                Base64Data = inGameData.Base64ResponseData!,
                SubmissionId = inGameData.SubmissionId != null ? Guid.Parse(inGameData.SubmissionId) : null,
                CollectedAt = now,
            };
            var date = DateOnly.FromDateTime(now);
            foreach (var pk in hohHelperResponseDtoToTablePkConverter.Get(inGameData, date))
            {
                await inGameRawDataTableRepository.SaveAsync(rawData, pk);
                logger.LogInformation("Saved raw data for primary key: {PrimaryKey}", pk);
            }

            logger.LogInformation("Processing raw data completed");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while processing raw data");
        }

        logger.LogInformation("Function InGameDataReceiver completed");
        return new NoContentResult();
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
