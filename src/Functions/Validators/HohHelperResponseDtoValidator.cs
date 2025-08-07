using Ingweland.Fog.Dtos.Hoh;

namespace Ingweland.Fog.Functions.Validators;

public class HohHelperResponseDtoValidator(
    EndpointValidator endpointValidator,
    PayloadValidator payloadValidator,
    WorldValidator worldIdValidator,
    SubmissionIdValidator submissionIdValidator)
{
    public bool Validate(HohHelperResponseDto dto, out string error)
    {
        error = string.Empty;

        if (!endpointValidator.ValidateEndpoint(dto.ResponseUrl, dto.CollectionCategoryIds, out error))
        {
            return false;
        }

        if (!payloadValidator.ValidatePayload(dto.Base64ResponseData, out error))
        {
            return false;
        }

        if (!worldIdValidator.ValidateWorld(dto.ResponseUrl, out error))
        {
            return false;
        }

        if (!submissionIdValidator.Validate(dto.SubmissionId, out error))
        {
            return false;
        }

        return true;
    }
}