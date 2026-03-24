namespace Ingweland.Fog.Application.Client.Web.Data.Entities;

public class HohLocalizationData
{
    public required string CultureCode { get; set; }
    public required byte[] Data { get; set; }
    public required string Id { get; set; }
    public required string Version { get; set; }

    public static class Keys
    {
        public const string ID = "id";
    }
}
