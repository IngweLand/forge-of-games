using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Functions.Validators;

public class PayloadValidator
{
    public bool ValidatePayload(string? base64ResponseData, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(base64ResponseData))
        {
            errorMessage = "Payload does not have response data.";
            return false;
        }

        return true;
    }
}