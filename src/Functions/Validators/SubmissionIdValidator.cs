namespace Ingweland.Fog.Functions.Validators;

public class SubmissionIdValidator
{
    public bool Validate(string? submissionId, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (submissionId == null)
        {
            return true;
        }
        
        if(Guid.TryParse(submissionId, out var guid) && guid != Guid.Empty)
        {
            return true;
        }
        
        errorMessage = "Submission id is not valid.";
        return false;
    }
}
