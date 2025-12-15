using Ingweland.Fog.Application.Server.Services.Hoh.Abstractions;
using Ingweland.Fog.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Validators;

public class PayloadValidator(IInGameDataParsingService inGameDataParsingService)
{
    public bool ValidatePayload(string? base64ResponseData, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(base64ResponseData))
        {
            errorMessage = "Payload does not have response data.";
            return false;
        }

        var softErrorResult = inGameDataParsingService.GetSoftError(base64ResponseData);
        if (softErrorResult.IsFailed)
        {
            softErrorResult.Log<PayloadValidator>(LogLevel.Error);
            errorMessage = "Could not parse payload";
            return false;
        }

        if (softErrorResult.Value != null)
        {
            errorMessage = $"Payload contains soft error: {softErrorResult.Value}";
            return false;
        }

        return true;
    }
}