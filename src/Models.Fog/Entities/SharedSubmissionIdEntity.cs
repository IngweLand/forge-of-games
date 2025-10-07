namespace Ingweland.Fog.Models.Fog.Entities;

public class SharedSubmissionIdEntity
{
    public required DateTime ExpiresAt { get; set; }
    public int Id { get; set; }
    public required Guid SharedId { get; set; }
    public required Guid SubmissionId { get; set; }
}
