namespace Ingweland.Fog.Dtos.Hoh;

public class ImageUploadDto
{
    public required string ContentType { get; set; }
    public required byte[] Data { get; set; }
}
